// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Numerics;
using System.Runtime.CompilerServices;

namespace BitmapRendering;

internal static class M4x4
{
    public static Matrix4x4 Create(Vector4 x, Vector4 y, Vector4 z, Vector4 w)
    {
        Unsafe.SkipInit(out Impl result);

        result.X = x;
        result.Y = y;
        result.Z = z;
        result.W = w;

        return Unsafe.BitCast<Impl, Matrix4x4>(result);
    }

    public static Matrix4x4 CreateFrom(OrthogonalTransform transform)
    {
        var result = Unsafe.BitCast<Matrix4x4, Impl>(Matrix4x4.CreateFromQuaternion(transform.Rotation));

        result.W = new Vector4(transform.Translation, 1);

        return Unsafe.BitCast<Impl, Matrix4x4>(result);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 GetX(this Matrix4x4 matrix) => Unsafe.BitCast<Matrix4x4, Impl>(matrix).X;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 GetY(this Matrix4x4 matrix) => Unsafe.BitCast<Matrix4x4, Impl>(matrix).Y;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 GetZ(this Matrix4x4 matrix) => Unsafe.BitCast<Matrix4x4, Impl>(matrix).Z;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 GetW(this Matrix4x4 matrix) => Unsafe.BitCast<Matrix4x4, Impl>(matrix).W;

    private struct Impl
    {
        public Vector4 X;
        public Vector4 Y;
        public Vector4 Z;
        public Vector4 W;
    }
}
