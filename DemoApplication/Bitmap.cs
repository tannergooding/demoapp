using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Mathematics;

using nuint = System.UInt64;

namespace DemoApplication
{
    public readonly struct Bitmap
    {
        #region Constants
        private const nuint BitsPerPixel = 32;
        private const nuint BytesPerBlock = 16;
        private const nuint BytesPerPixel = BitsPerPixel / 8;
        private const nuint PixelsPerBlock = BytesPerBlock / BytesPerPixel;

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

            var length = (nuint)PixelCount;

            if (useHWIntrinsics && (length >= PixelsPerBlock))
            {
                var vColor = Vector128.Create(color);
                AlignedStoreNonTemporal128(pRenderBuffer, length, vColor);

                var vDepth = Vector128.Create(depth);
                AlignedStoreNonTemporal128(pDepthBuffer, length, vDepth);
            }
            else
            {
                for (nuint index = 0; index < length; index++)
                {
                    pRenderBuffer[index] = color;
                    pDepthBuffer[index] = depth;
                }
            }
        }

        // This is based on "Bresenham’s Line  Generation Algorithm with Built-in Clipping - Yevgeny P. Kuzmin"
        public void DrawLine2D(Vector3 point1, Vector3 point2, uint color, bool useHWIntrinsics)
        {
            if (PixelCount == 0)
            {
                return;
            }

            // Draw from top to bottom to reduce the cases that need handled and to ensure
            // a deterministic line is drawn for the same endpoints. We also prefer drawing
            // from left to right, in the scenario where y1 = y2.

            var (sx1, sy1) = ((int)point1.X, (int)point1.Y);
            var (sx2, sy2) = ((int)point2.X, (int)point2.Y);

            if ((sy1 >= sy2) && ((sy1 != sy2) || (sx1 >= sx2)))
            {
                sx2 = Exchange(ref sx1, sx2);
                sy2 = Exchange(ref sy1, sy2);
            }

            if (sx1 == sx2)
            {
                if (sy1 == sy2)
                {
                    var point = new Vector3(point1.X, point1.Y, Math.Max(point1.Z, point2.Z));
                    DrawPixel(point, color);
                }
                else
                {
                    DrawVerticalLine2D(sx1, sy1, sy2, color);
                }
            }
            else if (sy1 == sy2)
            {
                DrawHorizontalLine2D(sx1, sy1, sx2, color, useHWIntrinsics);
            }
            else 
            {
                DrawDiagonalLine2D(sx1, sy1, sx2, sy2, color);
            }
        }

        public unsafe void DrawPixel(Vector3 point, uint color)
        {
            var (width, height) = (PixelWidth, PixelHeight);
            var (sx, sy) = ((int)point.X, (int)point.Y);

            if ((unchecked((uint)sx) >= width) || (unchecked((uint)sy) >= height))
            {
                return;
            }

            var index = (sy * width) + sx;
            DrawPixelUnsafe(index, color, point.Z);
        }

        public void DrawModel(Model model, uint color, bool isWireframe, bool useHWIntrinsics)
        {
            var verticeGroupCount = model.VerticeGroups.Count;

            for (var i = 0; i < verticeGroupCount; i++)
            {
                if (!isWireframe)
                {
                    if (ShouldCull(model, i))
                    {
                        continue;
                    }

                    var grey = (uint)((0.25f + ((i % verticeGroupCount) * 0.75f / verticeGroupCount)) * byte.MaxValue);
                    color = 0xFF000000 | (grey << 16) | (grey << 8) | grey;
                }

                var vertices = model.ModifiedVertices;
                var verticeGroup = model.VerticeGroups[i];
                var verticeCount = verticeGroup.Length;

                switch (verticeCount)
                {
                    case 1:
                    {
                        DrawPixel(vertices[verticeGroup[0]], color);
                        break;
                    }

                    case 2:
                    {
                        // TODO: Support 3D lines
                        DrawLine2D(vertices[verticeGroup[0]], vertices[verticeGroup[1]], color, useHWIntrinsics);
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
            if (isWireframe)
            {
                DrawLine2D(point1, point2, color, useHWIntrinsics);
                DrawLine2D(point2, point3, color, useHWIntrinsics);
                DrawLine2D(point3, point1, color, useHWIntrinsics);

                return;
            }

            ref var pt1 = ref point1;
            ref var pt2 = ref point2;
            ref var pt3 = ref point3;

            var sy1 = (int)point1.Y;
            var sy2 = (int)point2.Y;
            var sy3 = (int)point3.Y;

            if (sy1 > sy2)
            {
                sy2 = Exchange(ref sy1, sy2);

                ref var temp = ref pt1;
                pt1 = ref pt2;
                pt2 = ref temp;
            }

            Debug.Assert(sy1 <= sy2);

            if (sy2 > sy3)
            {
                sy3 = Exchange(ref sy2, sy3);

                ref var temp = ref pt2;
                pt2 = ref pt3;
                pt3 = ref temp;
            }

            Debug.Assert(sy1 <= sy3);
            Debug.Assert(sy2 <= sy3);

            if (sy1 > sy2)
            {
                sy2 = Exchange(ref sy1, sy2);

                ref var temp = ref pt1;
                pt1 = ref pt2;
                pt2 = ref temp;
            }

            Debug.Assert(sy1 <= sy2);
            Debug.Assert(sy2 <= sy3);

            if ((((pt2.X - pt1.X) * (pt3.Y - pt1.Y)) - ((pt3.X - pt1.X) * (pt2.Y - pt1.Y))) > 0)
            {
                DrawRightTriangle(pt1, pt2, pt3, color, useHWIntrinsics);
            }
            else
            {
                DrawLeftTriangle(pt1, pt2, pt3, color, useHWIntrinsics);
            }
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

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static unsafe void AlignedStoreNonTemporal128<T>(T* pDst, nuint length, Vector128<T> value)
            where T : unmanaged
        {
            Debug.Assert(Sse2.IsSupported);
            Debug.Assert(length >= PixelsPerBlock);

            var address = (nuint)pDst;
            var misalignment = address % BytesPerBlock;

            Debug.Assert((misalignment % PixelsPerBlock) == 0);

            if (misalignment != 0)
            {
                Sse2.Store((byte*)pDst, value.AsByte());
                misalignment = PixelsPerBlock - (misalignment / PixelsPerBlock);

                Debug.Assert(misalignment > 0);
                Debug.Assert(misalignment < PixelsPerBlock);

                pDst += misalignment;
                length -= misalignment;
            }

            Debug.Assert(((nuint)pDst % PixelsPerBlock) == 0);
            var remainder = length;

            if (length >= PixelsPerBlock)
            {
                remainder %= PixelsPerBlock;

                for (var pEnd = pDst + (length - remainder); pDst < pEnd; pDst += PixelsPerBlock)
                {
                    Sse2.StoreAlignedNonTemporal((byte*)pDst, value.AsByte());
                }
            }

            if (remainder != 0)
            {
                misalignment = PixelsPerBlock - remainder;
                pDst -= misalignment;
                Sse2.Store((byte*)pDst, value.AsByte());
            }
        }

        private static T Exchange<T>(ref T location, T value)
        {
            var temp = location;
            location = value;
            return temp;
        }

        private static float Lerp(float min, float max, float gradient)
        {
            return min + ((max - min) * Math.Clamp(gradient, 0.0f, 1.0f));
        }

        private void DrawDiagonalLine2D(int sx1, int sy1, int sx2, int sy2, uint color)
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
                var index = (d2 * PixelWidth) + d1;
                DrawPixelUnsafe(index, color, depth: 1.0f);

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

        private unsafe void DrawHorizontalLine2D(int sx1, int sy, int sx2, uint color, bool useHWIntrinsics)
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

            var index = (sy * width) + startX;
            var length = endX - startX;
            Debug.Assert(length >= 0);

            if (useHWIntrinsics && ((nuint)length >= PixelsPerBlock))
            {
                var pRenderBuffer = (uint*)RenderBuffer.BackBuffer;
                var vColor = Vector128.Create(color);
                AlignedStoreNonTemporal128(pRenderBuffer + index, (nuint)length, vColor);
            
                var pDepthBuffer = (float*)DepthBuffer.BackBuffer;
                var vDepth = Vector128.Create(1.0f);
                AlignedStoreNonTemporal128(pDepthBuffer + index, (nuint)length, vDepth);
            }
            else
            {
                var lastIndex = index + length;

                while (index < lastIndex)
                {
                    DrawPixelUnsafe(index, color, depth: 1.0f);
                    index++;
                }
            }
        }

        private unsafe void DrawHorizontalLine3D(int sy, Vector3 pa, Vector3 pb, Vector3 pc, Vector3 pd, uint color, bool useHWIntrinsics)
        {
            var g1 = pa.Y != pb.Y ? (sy - pa.Y) / (pb.Y - pa.Y) : 1.0f;
            var x1 = Lerp(pa.X, pb.X, g1);
            var z1 = Lerp(pa.Z, pb.Z, g1);

            var g2 = pc.Y != pd.Y ? (sy - pc.Y) / (pd.Y - pc.Y) : 1.0f;
            var x2 = Lerp(pc.X, pd.X, g2);
            var z2 = Lerp(pc.Z, pd.Z, g2);

            var sx1 = (int)x1;
            var sx2 = (int)x2;

            if (sx1 > sx2)
            {
                sx2 = Exchange(ref sx1, sx2);
                z2 = Exchange(ref z1, z2);
            }

            Debug.Assert(sx1 <= sx2);

            var (width, height) = (PixelWidth, PixelHeight);

            if ((unchecked((uint)sy) >= height) || (sx2 < 0) || (sx1 >= width))
            {
                return;
            }

            var startX = Math.Max(sx1, 0);
            var endX = Math.Min(sx2, width - 1);

            var startIndex = (sy * width) + startX;
            var length = endX - startX;
            Debug.Assert(length >= 0);

            var delta = (float)(startX - endX);

            var lastIndex = startIndex + length;

            for (var index = startIndex; index < lastIndex; index++)
            {
                var g3 = (index - startIndex) / delta;
                var depth = Lerp(z1, z2, g3);
                DrawPixelUnsafe(index, color, depth);
            }
        }

        private void DrawLeftTriangle(Vector3 pt1, Vector3 pt2, Vector3 pt3, uint color, bool useHWIntrinsics)
        {
            var sy1 = (int)pt1.Y;
            var sy2 = (int)pt2.Y;
            var sy3 = (int)pt3.Y;

            Debug.Assert(sy1 <= sy2);
            Debug.Assert(sy2 <= sy3);

            for (var sy = sy1; sy < sy2; sy++)
            {
                DrawHorizontalLine3D(sy, pt1, pt2, pt1, pt3, color, useHWIntrinsics);
            }

            for (var sy = sy2; sy <= sy3; sy++)
            {
                DrawHorizontalLine3D(sy, pt2, pt3, pt1, pt3, color, useHWIntrinsics);
            }
        }

        private unsafe void DrawPixelUnsafe(int index, uint color, float depth)
        {
            Debug.Assert((index >= 0) && (index < PixelCount));

            var pDepthBuffer = (float*)DepthBuffer.BackBuffer;

            if (pDepthBuffer[index] >= depth)
            {
                return;
            }
            pDepthBuffer[index] = depth;

            var pRenderBuffer = (uint*)RenderBuffer.BackBuffer;
            pRenderBuffer[index] = color;
        }

        private void DrawRightTriangle(Vector3 pt1, Vector3 pt2, Vector3 pt3, uint color, bool useHWIntrinsics)
        {
            var sy1 = (int)pt1.Y;
            var sy2 = (int)pt2.Y;
            var sy3 = (int)pt3.Y;

            Debug.Assert(sy1 <= sy2);
            Debug.Assert(sy2 <= sy3);

            for (var sy = sy1; sy < sy2; sy++)
            {
                DrawHorizontalLine3D(sy, pt1, pt3, pt1, pt2, color, useHWIntrinsics);
            }

            for (var sy = sy2; sy <= sy3; sy++)
            {
                DrawHorizontalLine3D(sy, pt1, pt3, pt2, pt3, color, useHWIntrinsics);
            }
        }

        private void DrawVerticalLine2D(int sx, int sy1, int sy2, uint color)
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
                DrawPixelUnsafe(index, color, depth: 1.0f);
                index += width;
            }
        }

        private bool ShouldCull(Model model, int index)
        {
            var normals = model.ModifiedNormals;
            var normalGroup = model.NormalGroups[index];
            var normalCount = normalGroup.Length;

            var center = Vector3.Zero;

            for (var n = 0; n < normalCount; n++)
            {
                center += normals[normalGroup[n]];
            }
            center /= normalGroup.Length;

            return ShouldCull(center.Normalize());
        }

        private bool ShouldCull(Vector3 normal)
        {
            return Vector3.DotProduct(normal, Vector3.UnitZ) <= 0;
        }
        #endregion
    }
}
