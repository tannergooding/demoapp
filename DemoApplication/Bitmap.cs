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

        public void DrawLine(Vector3 p1, Vector3 p2, uint color)
        {
            float xMin, xMax, yMin, yMax;

            if (p1.Y > p2.Y)
            {
                xMin = p2.X;
                xMax = p1.X;

                yMin = p2.Y;
                yMax = p1.Y;
            }
            else
            {
                xMin = p1.X;
                xMax = p2.X;

                yMin = p1.Y;
                yMax = p2.Y;
            }

            var xDelta = (int)(xMax - xMin);
            var yDelta = (int)(yMax - yMin);
            var direction = 1;
            if (xDelta < 0)
            { xDelta = -xDelta; direction = -1; }
            var xPos = xMin;
            var yPos = yMin;

            if (xDelta == 0)
            {
                for (var index = 0; index <= yDelta; index++)
                {
                    DrawPixel(xPos, yPos, color);
                    yPos += 1.0f;
                }
            }
            else if (yDelta == 0)
            {
                for (var index = 0; index <= xDelta; index++)
                {
                    DrawPixel(xPos, yPos, color);
                    xPos += direction;
                }
            }
            else if (xDelta == yDelta)
            {
                for (var index = 0; index <= xDelta; index++)
                {
                    DrawPixel(xPos, yPos, color);
                    xPos += direction;
                    yPos += 1.0f;
                }
            }
            else if (xDelta > yDelta)
            {
                var pixelsPerStep = xDelta / yDelta;
                var initialPixelStep = (pixelsPerStep / 2) + 1;
                var adjustUp = xDelta % yDelta * 2;
                var adjustDown = yDelta * 2;
                var errorTerm = (xDelta % yDelta) - (yDelta * 2);

                if ((adjustUp == 0) && ((pixelsPerStep & 1) == 0))
                { initialPixelStep -= 1; }
                if ((pixelsPerStep & 1) != 0)
                { errorTerm += yDelta; }

                for (var pixelsDrawn = 0; pixelsDrawn < initialPixelStep; pixelsDrawn++)
                {
                    DrawPixel(xPos, yPos, color);
                    xPos += direction;
                }
                yPos += 1.0f;

                for (var index = 0; index < (yDelta - 1); index++)
                {
                    var runLength = pixelsPerStep;
                    errorTerm += adjustUp;
                    if (errorTerm > 0)
                    { runLength++; errorTerm -= adjustDown; }

                    for (var pixelsDrawn = 0; pixelsDrawn < runLength; pixelsDrawn++)
                    {
                        DrawPixel(xPos, yPos, color);
                        xPos += direction;
                    }
                    yPos += 1.0f;
                }

                for (var pixelsDrawn = 0; pixelsDrawn < initialPixelStep; pixelsDrawn++)
                {
                    DrawPixel(xPos, yPos, color);
                    xPos += direction;
                }
            }
            else
            {
                var pixelsPerStep = yDelta / xDelta;
                var initialPixelStep = (pixelsPerStep / 2) + 1;
                var adjustUp = yDelta % xDelta * 2;
                var adjustDown = xDelta * 2;
                var errorTerm = (yDelta % xDelta) - (xDelta * 2);

                if ((adjustUp == 0) && ((pixelsPerStep & 1) == 0))
                { initialPixelStep -= 1; }
                if ((pixelsPerStep & 1) != 0)
                { errorTerm += xDelta; }

                for (var pixelsDrawn = 0; pixelsDrawn < initialPixelStep; pixelsDrawn++)
                {
                    DrawPixel(xPos, yPos, color);
                    yPos += 1.0f;
                }
                xPos += direction;

                for (var index = 0; index < (xDelta - 1); index++)
                {
                    var runLength = pixelsPerStep;
                    errorTerm += adjustUp;
                    if (errorTerm > 0)
                    { runLength++; errorTerm -= adjustDown; }

                    for (var pixelsDrawn = 0; pixelsDrawn < runLength; pixelsDrawn++)
                    {
                        DrawPixel(xPos, yPos, color);
                        yPos += 1.0f;
                    }
                    xPos += direction;
                }

                for (var pixelsDrawn = 0; pixelsDrawn < initialPixelStep; pixelsDrawn++)
                {
                    DrawPixel(xPos, yPos, color);
                    yPos += 1.0f;
                }
            }
        }

        public unsafe void DrawPixel(float xPos, float yPos, uint color)
        {
            var pixelIndex = ((int)yPos * Buffer.PixelWidth) + (int)xPos;

            if ((pixelIndex >= 0) && (pixelIndex < PixelCount))
            {
                var pRenderBuffer = (uint*)Buffer.BackBuffer;
                pRenderBuffer[pixelIndex] = color;
            }
        }

        public void DrawModel(Model model, uint color, bool isTriangles, bool isCulling)
        {
            for (var i = 0; i < model.VerticeGroups.Count; i++)
            {
                switch (model.VerticeGroups[i].Length)
                {
                    case 1:
                    {
                        if ((isCulling == false) || (ShouldCull(model, i) == false))
                        {
                            DrawPixel(model.ModifiedVertices[model.VerticeGroups[i][0]].X, model.ModifiedVertices[model.VerticeGroups[i][0]].Y, color);
                        }
                        break;
                    }

                    case 2:
                    {
                        if ((isCulling == false) || (ShouldCull(model, i) == false))
                        {
                            DrawLine(model.ModifiedVertices[model.VerticeGroups[i][0]], model.ModifiedVertices[model.VerticeGroups[i][1]], color);
                        }
                        break;
                    }

                    case 3:
                    {
                        if ((isCulling == false) || (ShouldCull(model, i) == false))
                        {
                            DrawTriangle(model.ModifiedVertices[model.VerticeGroups[i][0]], model.ModifiedVertices[model.VerticeGroups[i][1]], model.ModifiedVertices[model.VerticeGroups[i][2]], color);
                        }
                        break;
                    }

                    case 4:
                    {
                        if ((isCulling == false) || (ShouldCull(model, i) == false))
                        {
                            DrawQuad(model.ModifiedVertices[model.VerticeGroups[i][0]], model.ModifiedVertices[model.VerticeGroups[i][1]], model.ModifiedVertices[model.VerticeGroups[i][2]], model.ModifiedVertices[model.VerticeGroups[i][3]], color, isTriangles);
                        }
                        break;
                    }

                    default:
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
        }

        public void DrawQuad(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, uint color, bool isTriangles)
        {
            DrawLine(p1, p2, color);
            DrawLine(p2, p3, color);
            DrawLine(p3, p4, color);
            DrawLine(p4, p1, color);

            if (isTriangles)
            {
                DrawLine(p2, p4, color);
            }
        }

        public void DrawTriangle(Vector3 p1, Vector3 p2, Vector3 p3, uint color)
        {
            DrawLine(p1, p2, color);
            DrawLine(p2, p3, color);
            DrawLine(p3, p1, color);
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

        private bool ShouldCull(Model model, int index)
        {
            switch (model.NormalGroups[index].Length)
            {
                case 1:
                {
                    var normal = model.ModifiedNormals[model.NormalGroups[index][0]];
                    normal /= normal.Length;
                    return ShouldCull(normal);
                }

                case 2:
                {

                    var normal = model.ModifiedNormals[model.NormalGroups[index][0]] + model.ModifiedNormals[model.NormalGroups[index][1]];
                    normal /= normal.Length;
                    return ShouldCull(normal);
                }

                case 3:
                {
                    var normal = model.ModifiedNormals[model.NormalGroups[index][0]] + model.ModifiedNormals[model.NormalGroups[index][1]] + model.ModifiedNormals[model.NormalGroups[index][2]];
                    normal /= normal.Length;
                    return ShouldCull(normal);
                }

                case 4:
                {
                    var normal = model.ModifiedNormals[model.NormalGroups[index][0]] + model.ModifiedNormals[model.NormalGroups[index][1]] + model.ModifiedNormals[model.NormalGroups[index][2]] + model.ModifiedNormals[model.NormalGroups[index][3]];
                    normal /= normal.Length;
                    return ShouldCull(normal);
                }

                default:
                {
                    throw new InvalidOperationException();
                }
            }
        }

        private bool ShouldCull(Vector3 normal)
        {
            return Vector3.DotProduct(normal, Vector3.UnitZ) >= 0;
        }
        #endregion
    }
}
