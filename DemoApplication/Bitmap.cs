using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Threading;
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

        private static readonly PixelFormat PixelFormat = PixelFormats.Bgra32;
        #endregion

        #region Fields
        public readonly WriteableBitmap Buffer;
        public readonly int PixelCount;
        #endregion

        #region Constructors
        public Bitmap(int width, int height)
        {
            Buffer = new WriteableBitmap(width, height, DpiX, DpiY, PixelFormat, palette: null);
            PixelCount = width * height;
        }
        #endregion

        #region Properties
        public int PixelHeight
        {
            get
            {
                return Buffer.PixelHeight;
            }
        }

        public int PixelWidth
        {
            get
            {
                return Buffer.PixelWidth;
            }
        }
        #endregion

        #region Methods
        public unsafe void Clear(uint color, bool useHWIntrinsics)
        {
            if (useHWIntrinsics)
            {
                if (Sse2.IsSupported)
                {
                    var pixelsPerWrite = Unsafe.SizeOf<Vector128<uint>>() / BytesPerPixel;
                    var pBackBuffer = (uint*)Buffer.BackBuffer;
                    var pixelCount = PixelCount;
                    var remainder = pixelCount % pixelsPerWrite;
                    var lastBlockIndex = pixelCount - remainder;
                    var vColor = Vector128.Create(color);

                    var index = 0;

                    while (index < lastBlockIndex)
                    {
                        Debug.Assert((index >= 0) && (index < pixelCount));
                        Sse2.Store(pBackBuffer + index, vColor);
                        index += pixelsPerWrite;
                    }

                    while (index < pixelCount)
                    {
                        pBackBuffer[index] = color;
                        index++;
                    }
                    return;
                }
            }

            for (var yPos = 0; yPos < Buffer.PixelHeight; yPos++)
            {
                for (var xPos = 0; xPos < Buffer.PixelWidth; xPos++)
                {
                    DrawPixel(xPos, yPos, color);
                }
            }
        }

        // This is based on "Bresenham’s Line  Generation Algorithm with Built-in Clipping - Yevgeny P. Kuzmin"
        public void DrawLine(Vector3 point1, Vector3 point2, uint color)
        {
            if (PixelCount == 0)
            {
                return;
            }

            // Draw from top to bottom to reduce the cases that need handled and to ensure
            // a deterministic line is drawn for the same endpoints. We also prefer drawing
            // from left to right, in the scenario where y1 = y2.

            var (x1, y1) = ((int)point1.X, (int)point1.Y);
            var (x2, y2) = ((int)point2.X, (int)point2.Y);

            if ((y1 >= y2) && ((y1 != y2) || (x1 >= x2)))
            {
                x2 = Interlocked.Exchange(ref x1, x2);
                y2 = Interlocked.Exchange(ref y1, y2);
            }

            if (x1 == x2)
            {
                if (y1 == y2)
                {
                    DrawPixel(x1, y1, color);
                }
                else
                {
                    DrawVerticalLine(x1, y1, y2, color);
                }
            }
            else if (y1 == y2)
            {
                DrawHorizontalLine(x1, y1, x2, color);
            }
            else 
            {
                DrawDiagonalLine(x1, y1, x2, y2, color);
            }
        }

        public void DrawPixel(int x, int y, uint color)
        {
            var (width, height) = (PixelWidth, PixelHeight);

            if ((x < 0) || (x >= width) || (y < 0) || (y >= height))
            {
                return;
            }

            var index = (y * width) + x;
            DrawPixelUnsafe(index, color);
        }

        public void DrawModel(Model model, uint color, bool isTriangles, bool isCulling)
        {
            for (var i = 0; i < model.VerticeGroups.Count; i++)
            {
                if (isCulling && ShouldCull(model, i))
                {
                    continue;
                }

                switch (model.VerticeGroups[i].Length)
                {
                    case 1:
                    {
                        DrawPixel((int)model.ModifiedVertices[model.VerticeGroups[i][0]].X, (int)model.ModifiedVertices[model.VerticeGroups[i][0]].Y, color);
                        break;
                    }

                    case 2:
                    {
                        DrawLine(model.ModifiedVertices[model.VerticeGroups[i][0]], model.ModifiedVertices[model.VerticeGroups[i][1]], color);
                        break;
                    }

                    case 3:
                    {
                        DrawTriangle(model.ModifiedVertices[model.VerticeGroups[i][0]], model.ModifiedVertices[model.VerticeGroups[i][1]], model.ModifiedVertices[model.VerticeGroups[i][2]], color);
                        break;
                    }

                    case 4:
                    {
                        DrawQuad(model.ModifiedVertices[model.VerticeGroups[i][0]], model.ModifiedVertices[model.VerticeGroups[i][1]], model.ModifiedVertices[model.VerticeGroups[i][2]], model.ModifiedVertices[model.VerticeGroups[i][3]], color, isTriangles);
                        break;
                    }

                    default:
                    {
                        for (var n = 0; n < (model.VerticeGroups[i].Length - 2); n++)
                        {
                            DrawLine(model.ModifiedVertices[model.VerticeGroups[i][n]], model.ModifiedVertices[model.VerticeGroups[i][n + 1]], color);
                        }
                        DrawLine(model.ModifiedVertices[model.VerticeGroups[i][model.VerticeGroups[i].Length - 1]], model.ModifiedVertices[model.VerticeGroups[i][0]], color);
                        break;
                    }
                }
            }
        }

        public void DrawQuad(Vector3 point1, Vector3 point2, Vector3 point3, Vector3 point4, uint color, bool isTriangles)
        {
            DrawLine(point1, point2, color);
            DrawLine(point2, point3, color);
            DrawLine(point3, point4, color);
            DrawLine(point4, point1, color);

            if (isTriangles)
            {
                DrawLine(point1, point3, color);
            }
        }

        public void DrawTriangle(Vector3 point1, Vector3 point2, Vector3 point3, uint color)
        {
            DrawLine(point1, point2, color);
            DrawLine(point2, point3, color);
            DrawLine(point3, point1, color);
        }

        public void Lock()
        {
            Buffer.Lock();
        }

        public void Invalidate()
        {
            var bufferRegion = new Int32Rect(0, 0, Buffer.PixelWidth, Buffer.PixelHeight);
            Buffer.AddDirtyRect(bufferRegion);
        }

        public void Unlock()
        {
            Buffer.Unlock();
        }

        private static T Exchange<T>(ref T location, T value)
        {
            var temp = location;
            location = value;
            return temp;
        }

        private void DrawDiagonalLine(int sx1, int sy1, int sx2, int sy2, uint color)
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
                DrawPixelUnsafe(index, color);

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

        private void DrawHorizontalLine(int x1, int y, int x2, uint color)
        {
            // We only support drawing left to right and expect the pixel case to have been handled
            Debug.Assert(x1 < x2);

            var (width, height) = (PixelWidth, PixelHeight);

            if ((y < 0) || (y >= height) || (x2 < 0) || (x1 >= width))
            {
                return;
            }

            var startX = Math.Max(x1, 0);
            var endX = Math.Min(x2, width - 1);

            var delta = endX - startX;
            Debug.Assert(delta >= 0);

            var index = (y * width) + startX;

            for (var i = 0; i < delta; i++, index++)
            {
                DrawPixelUnsafe(index, color);
            }
        }

        private unsafe void DrawPixelUnsafe(int index, uint color)
        {
            Debug.Assert((index >= 0) && (index < PixelCount));
            var pBackBuffer = (uint*)Buffer.BackBuffer;
            pBackBuffer[index] = color;
        }

        private void DrawVerticalLine(int x, int y1, int y2, uint color)
        {
            // We only support drawing top to bottom and expect the pixel case to have been handled
            Debug.Assert(y1 < y2);

            var (width, height) = (PixelWidth, PixelHeight);

            if ((x < 0) || (x >= width) || (y2 < 0) || (y1 >= height))
            {
                return;
            }

            var startY = Math.Max(y1, 0);
            var endY = Math.Min(y2, height - 1);

            var delta = endY - startY;
            Debug.Assert(delta >= 0);

            var index = (startY * width) + x;

            for (var i = 0; i < delta; i++, index += width)
            {
                DrawPixelUnsafe(index, color);
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
                normal = normal + model.ModifiedNormals[model.NormalGroups[index][0]];
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
