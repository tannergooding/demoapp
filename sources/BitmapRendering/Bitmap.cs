// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using Mathematics;

namespace BitmapRendering
{
    public readonly struct Bitmap
    {
        private const int BitsPerPixel = 32;
        private const int BytesPerBlock = 16;
        private const int BytesPerPixel = BitsPerPixel / 8;
        private const int PixelsPerBlock = BytesPerBlock / BytesPerPixel;

        private const double DpiX = 96.0;
        private const double DpiY = 96.0;

        public readonly IntPtr RenderBuffer;
        public readonly IntPtr DepthBuffer;
        public readonly int PixelWidth;
        public readonly int PixelHeight;
        public readonly int PixelCount;

        public Bitmap(IntPtr renderBuffer, IntPtr depthBuffer, int width, int height)
        {
            RenderBuffer = renderBuffer;
            DepthBuffer = depthBuffer;
            PixelWidth = width;
            PixelHeight = height;
            PixelCount = width * height;
        }

        public unsafe void Clear(uint color, float depth, bool useHWIntrinsics)
        {
            var length = (nuint)PixelCount;

            var pRenderBuffer = (uint*)RenderBuffer;
            var pDepthBuffer = (float*)DepthBuffer;

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
                }

                for (nuint index = 0; index < length; index++)
                {
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

        public void DrawModel(Model model, Vector3 lightPosition, uint color, bool isWireframe, bool useHWIntrinsics)
        {
            var verticeGroupCount = model.VerticeGroups.Count;

            for (var i = 0; i < verticeGroupCount; i++)
            {
                var normal = Vector3.Zero;

                if (!isWireframe && ShouldCull(model, i, out normal))
                {
                    continue;
                }

                var vertices = model.ModifiedVertices;
                var verticeGroup = model.VerticeGroups[i];
                var verticeCount = verticeGroup.Length;

                var center = Vector3.Zero;

                for (var n = 0; n < verticeCount; n++)
                {
                    center += vertices[verticeGroup[n]];
                }
                center /= verticeGroup.Length;

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
                        DrawTriangle(vertices[verticeGroup[0]], vertices[verticeGroup[1]], vertices[verticeGroup[2]], center, normal, lightPosition, color, isWireframe, useHWIntrinsics);
                        break;
                    }

                    case 4:
                    {
                        DrawTriangle(vertices[verticeGroup[0]], vertices[verticeGroup[1]], vertices[verticeGroup[2]], center, normal, lightPosition, color, isWireframe, useHWIntrinsics);
                        DrawTriangle(vertices[verticeGroup[2]], vertices[verticeGroup[3]], vertices[verticeGroup[0]], center, normal, lightPosition, color, isWireframe, useHWIntrinsics);
                        break;
                    }

                    default:
                    {
                        for (var n = 0; n < (verticeCount - 1); n++)
                        {
                            DrawTriangle(vertices[verticeGroup[n]], vertices[verticeGroup[n + 1]], center, center, normal, lightPosition, color, isWireframe, useHWIntrinsics);
                        }
                        DrawTriangle(vertices[verticeGroup[verticeCount - 1]], vertices[verticeGroup[0]], center, center, normal, lightPosition, color, isWireframe, useHWIntrinsics);

                        break;
                    }
                }
            }
        }

        public void DrawTriangle(Vector3 point1, Vector3 point2, Vector3 point3, Vector3 center, Vector3 normal, Vector3 lightPosition, uint color, bool isWireframe, bool useHWIntrinsics)
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

            var lightDirection = (lightPosition - center).Normalize();
            var ndotl = Math.Max(0, Vector3.DotProduct(normal, lightDirection));

            var blue = unchecked((byte)color) * ndotl;
            var green = unchecked((byte)(color >> 8)) * ndotl;
            var red = unchecked((byte)(color >> 16)) * ndotl;

            color = 0xFF000000 | ((uint)red << 16) | ((uint)green << 8) | (uint)blue;

            if ((((pt2.X - pt1.X) * (pt3.Y - pt1.Y)) - ((pt3.X - pt1.X) * (pt2.Y - pt1.Y))) > 0)
            {
                DrawRightTriangle(pt1, pt2, pt3, color, useHWIntrinsics);
            }
            else
            {
                DrawLeftTriangle(pt1, pt2, pt3, color, useHWIntrinsics);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static unsafe void AlignedStoreNonTemporal128<T>(T* pDst, nuint length, Vector128<T> value)
            where T : unmanaged
        {
            Debug.Assert(length >= PixelsPerBlock);

            var address = (nuint)pDst;
            var misalignment = address % BytesPerBlock;

            Debug.Assert((misalignment % PixelsPerBlock) == 0);

            if (misalignment != 0)
            {
                value.Store(pDst);
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
                    value.StoreAlignedNonTemporal(pDst);
                }
            }

            if (remainder != 0)
            {
                misalignment = PixelsPerBlock - remainder;
                pDst -= misalignment;
                value.Store(pDst);
            }
        }

        private static T Exchange<T>(ref T location, T value)
        {
            var temp = location;
            location = value;
            return temp;
        }

        private static float Interpolate(float min, float max, float gradient)
        {
            var t = Math.Min(Math.Max(gradient, 0), 1);
            return ((1 - t) * min) + (t * max);
        }

        private static Vector128<float> Interpolate(Vector128<float> min, Vector128<float> max, Vector128<float> gradient)
        {
            var t = Vector128.Min(Vector128.Max(gradient, Vector128<float>.Zero), Vector128<float>.One);
            return ((Vector128<float>.One - t) * min) + (t * max);
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
                if ((sx2 > wx2) || (sx1 < wx1))
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
                var tmp = (dx2 * (wy2 - sy1)) + dsx;
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
                var pRenderBuffer = (uint*)RenderBuffer;
                var vColor = Vector128.Create(color);
                AlignedStoreNonTemporal128(pRenderBuffer + index, (nuint)length, vColor);

                var pDepthBuffer = (float*)DepthBuffer;
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
            var x1 = Interpolate(pa.X, pb.X, g1);

            var g2 = pc.Y != pd.Y ? (sy - pc.Y) / (pd.Y - pc.Y) : 1.0f;
            var x2 = Interpolate(pc.X, pd.X, g2);

            var sx1 = (int)x1;
            var sx2 = (int)x2;

            var sz1 = Interpolate(pa.Z, pb.Z, g1);
            var sz2 = Interpolate(pc.Z, pd.Z, g2);

            if (sx1 > sx2)
            {
                sx2 = Exchange(ref sx1, sx2);
                sz2 = Exchange(ref sz1, sz2);
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

            if (useHWIntrinsics && ((nuint)length >= PixelsPerBlock))
            {
                var vz1 = Vector128.Create(sz1);
                var vz2 = Vector128.Create(sz2);

                var vColor = Vector128.Create(color);
                var vIndex = Vector128.Create(0, 1, 2, 3);
                var vDelta = Vector128.Create(delta);

                var remainder = length % (int)PixelsPerBlock;

                var pRenderBuffer = (uint*)RenderBuffer + startIndex;
                var pDepthBuffer = (float*)DepthBuffer + startIndex;

                for (var pEnd = pRenderBuffer + (length - remainder); pRenderBuffer < pEnd; pRenderBuffer += PixelsPerBlock, pDepthBuffer += PixelsPerBlock)
                {
                    var vg3 = Vector128.ConvertToSingle(vIndex) / vDelta;
                    var vDepth = Interpolate(vz1, vz2, vg3);

                    // Load the existing colors/depths
                    var eColor = Vector128.Load(pRenderBuffer);
                    var eDepth = Vector128.Load(pDepthBuffer);

                    var mask = Vector128.GreaterThanOrEqual(eDepth, vDepth);

                    var nColor = Vector128.ConditionalSelect(mask.AsUInt32(), eColor, vColor);  // nColor = (eDepth >= vDepth) ? eDepth : vDepth;
                    var nDepth = Vector128.ConditionalSelect(mask, eDepth, vDepth);             // nDepth = (eDepth >= vDepth) ? eDepth : vDepth;

                    nColor.Store(pRenderBuffer);
                    nDepth.Store(pDepthBuffer);

                    vIndex += Vector128.Create(PixelsPerBlock);
                }

                for (var index = lastIndex - remainder; index < lastIndex; index++)
                {
                    var g3 = (index - startIndex) / delta;
                    var depth = Interpolate(sz1, sz2, g3);
                    DrawPixelUnsafe(index, color, depth);
                }
            }
            else
            {
                for (var index = startIndex; index < lastIndex; index++)
                {
                    var g3 = (index - startIndex) / delta;
                    var depth = Interpolate(sz1, sz2, g3);
                    DrawPixelUnsafe(index, color, depth);
                }
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

            var pDepthBuffer = (float*)DepthBuffer;

            if (pDepthBuffer[index] >= depth)
            {
                return;
            }
            pDepthBuffer[index] = depth;

            var pRenderBuffer = (uint*)RenderBuffer;
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

        private bool ShouldCull(Model model, int index, out Vector3 normal)
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

            normal = center.Normalize();
            return ShouldCull(normal);
        }

        private bool ShouldCull(Vector3 normal) => Vector3.DotProduct(normal, Vector3.UnitZ) <= 0;
    }
}
