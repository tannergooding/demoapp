using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Mathematics;

namespace DemoApplication
{
    public readonly struct Bitmap
    {
        #region Constants
        private const int BitsPerPixel = 32;
        private const int BytesPerPixel = BitsPerPixel / 8;

        private const double DpiX = 96.0;
        private const double DpiY = 96.0;

        private static readonly PixelFormat RenderBufferPixelFormat = PixelFormats.Bgra32;
        private static readonly PixelFormat DepthBufferPixelFormat = PixelFormats.Gray32Float;
        #endregion

        #region Fields
        public readonly WriteableBitmap RenderBuffer;
        public readonly WriteableBitmap DepthBuffer;
        public readonly int PixelCount;
        #endregion

        #region Constructors
        public Bitmap(int width, int height)
        {
            RenderBuffer = new WriteableBitmap(width, height, DpiX, DpiY, RenderBufferPixelFormat, palette: null);
            DepthBuffer = new WriteableBitmap(width, height, DpiX, DpiY, DepthBufferPixelFormat, palette: null);
            PixelCount = width * height;
        }
        #endregion

        #region Properties
        public int PixelHeight
        {
            get
            {
                return RenderBuffer.PixelHeight;
            }
        }

        public int PixelWidth
        {
            get
            {
                return RenderBuffer.PixelWidth;
            }
        }
        #endregion

        #region Methods
        public unsafe void Clear(uint color, float depth, bool useHWIntrinsics)
        {
            var pRenderBuffer = (uint*)RenderBuffer.BackBuffer;
            var pDepthBuffer = (float*)DepthBuffer.BackBuffer;

            if (useHWIntrinsics)
            {
                if (Sse2.IsSupported)
                {
                    var pixelsPerWrite = Unsafe.SizeOf<Vector128<uint>>() / BytesPerPixel;
                    var pixelCount = PixelCount;
                    var remainder = pixelCount % pixelsPerWrite;
                    var lastBlockIndex = pixelCount - remainder;

                    var vColor = Vector128.Create(color);
                    var vDepth = Vector128.Create(depth);

                    var index = 0;

                    while (index < lastBlockIndex)
                    {
                        Debug.Assert((index >= 0) && (index < pixelCount));
                        Sse2.Store(pRenderBuffer + index, vColor);
                        Sse.Store(pDepthBuffer + index, vDepth);
                        index += pixelsPerWrite;
                    }

                    while (index < pixelCount)
                    {
                        pRenderBuffer[index] = color;
                        pDepthBuffer[index] = depth;
                        index++;
                    }
                    return;
                }
            }

            for (var index = 0; index < PixelCount; index++)
            {
                pRenderBuffer[index] = color;
                pDepthBuffer[index] = depth;
            }
        }

        // This is based on "Bresenham’s Line  Generation Algorithm with Built-in Clipping - Yevgeny P. Kuzmin"
        public void DrawLine(Vector3 point1, Vector3 point2, uint color, bool useHWIntrinsics)
        {
            if (PixelCount == 0)
            {
                return;
            }

            // Draw from top to bottom to reduce the cases that need handled and to ensure
            // a deterministic line is drawn for the same endpoints. We also prefer drawing
            // from left to right, in the scenario where y1 = y2.

            var (sx1, sy1, sz1) = ((int)point1.X, (int)point1.Y, point1.Z);
            var (sx2, sy2, sz2) = ((int)point2.X, (int)point2.Y, point2.Z);

            if ((sy1 >= sy2) && ((sy1 != sy2) || (sx1 >= sx2)))
            {
                sx2 = Exchange(ref sx1, sx2);
                sy2 = Exchange(ref sy1, sy2);
                sz2 = Exchange(ref sz1, sz2);
            }

            if (sx1 == sx2)
            {
                if (sy1 == sy2)
                {
                    DrawPixel(sx1, sy1, color, depth: Math.Max(sz1, sz2));
                }
                else
                {
                    DrawVerticalLine(sx1, sy1, sz1, sy2, sz2, color);
                }
            }
            else if (sy1 == sy2)
            {
                DrawHorizontalLine(sx1, sy1, sz1, sx2, sz2, color, useHWIntrinsics);
            }
            else 
            {
                DrawDiagonalLine(sx1, sy1, sz1, sx2, sy2, sz2, color, useHWIntrinsics);
            }
        }

        public unsafe void DrawPixel(int sx, int sy, uint color, float depth)
        {
            var (width, height) = (PixelWidth, PixelHeight);

            if ((unchecked((uint)sx) >= width) || (unchecked((uint)sy) >= height))
            {
                return;
            }

            var index = (sy * width) + sx;
            DrawPixelUnsafe(index, color, depth);
        }

        public void DrawModel(Model model, uint color, bool isWireframe, bool useHWIntrinsics)
        {
            for (var i = 0; i < model.VerticeGroups.Count; i++)
            {
                if (!isWireframe && ShouldCull(model, i))
                {
                    continue;
                }

                var vertices = model.ModifiedVertices;
                var verticeGroup = model.VerticeGroups[i];
                var verticeCount = verticeGroup.Length;

                switch (verticeCount)
                {
                    case 1:
                    {
                        var point = vertices[verticeGroup[0]];
                        DrawPixel((int)point.X, (int)point.Y, color, point.Z);
                        break;
                    }

                    case 2:
                    {
                        DrawLine(vertices[verticeGroup[0]], vertices[verticeGroup[1]], color, useHWIntrinsics);
                        break;
                    }

                    case 3:
                    {
                        DrawTriangle(vertices[verticeGroup[0]], vertices[verticeGroup[1]], vertices[verticeGroup[2]], color, isWireframe, useHWIntrinsics);
                        break;
                    }

                    default:
                    {
                        var center = Vector3.Zero;

                        for (var n = 0; n < verticeCount; n++)
                        {
                            center += vertices[verticeGroup[n]];
                        }
                        center /= verticeGroup.Length;

                        for (var n = 0; n < (verticeCount - 1); n++)
                        {
                            DrawTriangle(vertices[verticeGroup[n]], vertices[verticeGroup[n + 1]], center, color, isWireframe, useHWIntrinsics);
                        }
                        DrawTriangle(vertices[verticeGroup[verticeCount - 1]], vertices[verticeGroup[0]], center, color, isWireframe, useHWIntrinsics);

                        break;
                    }
                }
            }
        }

        public void DrawTriangle(Vector3 point1, Vector3 point2, Vector3 point3, uint color, bool isWireframe, bool useHWIntrinsics)
        {
            DrawLine(point1, point2, color, useHWIntrinsics);
            DrawLine(point2, point3, color, useHWIntrinsics);
            DrawLine(point3, point1, color, useHWIntrinsics);
        }

        public void Lock()
        {
            RenderBuffer.Lock();
            DepthBuffer.Lock();
        }

        public void Invalidate()
        {
            var bufferRegion = new Int32Rect(0, 0, RenderBuffer.PixelWidth, RenderBuffer.PixelHeight);
            RenderBuffer.AddDirtyRect(bufferRegion);
            DepthBuffer.AddDirtyRect(bufferRegion);
        }

        public void Unlock()
        {
            RenderBuffer.Unlock();
            DepthBuffer.Unlock();
        }

        private static T Exchange<T>(ref T location, T value)
        {
            var temp = location;
            location = value;
            return temp;
        }

        private void DrawDiagonalLine(int sx1, int sy1, float sz1, int sx2, int sy2, float sz2, uint color, bool useHWIntrinsics)
        {
            // We only support drawing top to bottom and left to right; We also expect
            // the horizontal and vertical cases to have already been handled

            Debug.Assert(sx1 != sx2);
            Debug.Assert(sy1 < sy2);

            var (width, height) = (PixelWidth, PixelHeight);

            // Window
            var wx1 = 0;
            var wy1 = 0;
            var wx2 = width - 1;
            var wy2 = height - 1;

            var stx = 1;
            var sty = 1;

            if (sx1 < sx2)
            {
                if ((sx1 > wx2) || (sx2 < wx1))
                {
                    return;
                }
            }
            else
            {
                if ((sx2 > wx2) || (sx2 < wx1))
                {
                    return;
                }

                sx1 = -sx1;
                sx2 = -sx2;

                wx1 = -wx1;
                wx2 = -wx2;

                wx2 = Exchange(ref wx1, wx2);

                stx = -stx;
            }

            if ((sy1 > wy2) || (sy2 < wy1))
            {
                return;
            }

            var dsx = sx2 - sx1;
            var dsy = sy2 - sy1;

            var xd = 0;
            var yd = 0;

            ref var d1 = ref xd;
            ref var d2 = ref yd;

            if (dsx < dsy)
            {
                // We start out assuming a primarily horizontal line, but
                // switch variables around as needed here if we end up being
                // primarily vertical instead.

                sy1 = Exchange(ref sx1, sy1);
                sy2 = Exchange(ref sx2, sy2);

                wy1 = Exchange(ref wx1, wy1);
                wy2 = Exchange(ref wx2, wy2);

                sty = Exchange(ref stx, sty);
                dsy = Exchange(ref dsx, dsy);

                d1 = ref yd;
                d2 = ref xd;
            }

            // Some general assertions that the paper maintains
            Debug.Assert((wx1 <= wx2) && (wy1 <= wy2));
            Debug.Assert((sx1 <= sx2) && (sy1 <= sy2));
            Debug.Assert(dsy <= dsx);

            var foundWindowExit = false;

            // Setup for Bresenham's Algorithm

            var dx2 = 2 * dsx;
            var dy2 = 2 * dsy;

            xd = sx1;
            yd = sy1;

            var e = (2 * dsy) - dsx;
            var term = sx2;

            if (sy1 < wy1)                              // Horizontal Entry
            {
                var tmp = (dx2 * (wy1 - sy1)) - dsx;
                xd += tmp / dy2;
                var rem = tmp % dy2;

                if (xd > wx2)
                {
                    return;
                }
                else if ((xd + 1) >= wx1)
                {
                    yd = wy1;
                    e -= rem + dsx;

                    if (rem > 0)
                    {
                        xd++;
                        e += dy2;
                    }

                    foundWindowExit = true;
                }
            }

            if ((!foundWindowExit) && (sx1 < wx1))      // Vertical Entry
            {
                var tmp = dy2 * (wx1 - sx1);
                yd += tmp / dx2;
                var rem = tmp % dx2;

                if ((yd > wy2) || ((yd == wy2) && (rem >= dsx)))
                {
                    return;
                }
                else
                {
                    xd = wx1;
                    e += rem;

                    if (rem >= dsx)
                    {
                        yd++;
                        e -= dx2;
                    }
                }
            }

            if (sy2 > wy2)                              // Window Exit
            {
                var tmp = dx2 * (wy2 - sy1) + dsx;
                term = sx1 + (tmp / dy2);
                var rem = tmp % dy2;

                if (rem == 0)
                {
                    term--;
                }
            }

            if (term > wx2)
            {
                term = wx2;
            }

            term++;

            if (sty == -1)
            {
                yd = -yd;                           // Reverse Transformation
            }

            if (stx == -1)
            {
                xd = -xd;                           // Reverse Transformation
                term = -term;
            }

            dx2 -= dy2;

            while (xd != term)                      // Bresenham's Line Drawing
            {
                // TODO: UseHWIntrinsics
                // TODO: Depth

                var index = (d2 * PixelWidth) + d1;
                DrawPixelUnsafe(index, color, 1.0f);

                if (e >= 0)
                {
                    xd += stx;
                    yd += sty;
                    e -= dx2;
                }
                else
                {
                    xd += stx;
                    e += dy2;
                }
            }
        }

        private void DrawHorizontalLine(int sx1, int sy, float sz1, int sx2, float sz2, uint color, bool useHWIntrinsics)
        {
            // We only support drawing left to right and expect the pixel case to have been handled
            Debug.Assert(sx1 < sx2);

            var (width, height) = (PixelWidth, PixelHeight);

            if ((unchecked((uint)sy) >= height) || (sx2 < 0) || (sx1 >= width))
            {
                return;
            }

            var startX = Math.Max(sx1, 0);
            var endX = Math.Min(sx2, width - 1);

            var delta = endX - startX;
            Debug.Assert(delta >= 0);

            var index = (sy * width) + startX;
            var lastIndex = index + delta;

            // TODO: UseHWIntrinsics

            while (index < lastIndex)
            {
                // TODO: Depth
                DrawPixelUnsafe(index, color, 1.0f);
                index++;
            }
        }

        private unsafe void DrawPixelUnsafe(int index, uint color, float depth)
        {
            Debug.Assert((index >= 0) && (index < PixelCount));

            var pDepthBuffer = (float*)DepthBuffer.BackBuffer;

            if (pDepthBuffer[index] > depth)
            {
                return;
            }
            pDepthBuffer[index] = depth;

            var pRenderBuffer = (uint*)RenderBuffer.BackBuffer;
            pRenderBuffer[index] = color;
        }

        private void DrawVerticalLine(int sx, int sy1, float sz1, int sy2, float sz2, uint color)
        {
            // We only support drawing top to bottom and expect the pixel case to have been handled
            Debug.Assert(sy1 < sy2);

            var (width, height) = (PixelWidth, PixelHeight);

            if ((unchecked((uint)sx) >= width) || (sy2 < 0) || (sy1 >= height))
            {
                return;
            }

            var startY = Math.Max(sy1, 0);
            var endY = Math.Min(sy2, height - 1);

            var delta = endY - startY;
            Debug.Assert(delta >= 0);

            var index = (startY * width) + sx;
            var lastIndex = index + (delta * width);

            while (index < lastIndex)
            {
                // TODO: Depth
                DrawPixelUnsafe(index, color, 1.0f);
                index += width;
            }
        }

        private bool ShouldCull(Model model, int index)
        {
            var normal = model.ModifiedNormals[model.NormalGroups[index][model.NormalGroups[index].Length - 1]];

            if (index != 1)
            {
                switch (model.NormalGroups[index].Length)
                {
                    case 2:
                    {
                        normal += model.ModifiedNormals[model.NormalGroups[index][0]];
                        break;
                    }

                    case 3:
                    {
                        normal += model.ModifiedNormals[model.NormalGroups[index][1]];
                        break;
                    }

                    default:
                    {
                        normal += model.ModifiedNormals[model.NormalGroups[index][2]];
                        break;
                    }
                }
            }
        
            if (index != 2)
            {
                switch (model.NormalGroups[index].Length)
                {
                    case 3:
                    {
                        normal += model.ModifiedNormals[model.NormalGroups[index][0]];
                        break;
                    }

                    default:
                    {
                        normal += model.ModifiedNormals[model.NormalGroups[index][1]];
                        break;
                    }
                }
            }

            if (index != 3)
            {
                normal += model.ModifiedNormals[model.NormalGroups[index][0]];
            }

            return ShouldCull(normal.Normalize());
        }

        private bool ShouldCull(Vector3 normal)
        {
            return Vector3.DotProduct(normal, Vector3.UnitZ) >= 0;
        }
        #endregion
    }
}
