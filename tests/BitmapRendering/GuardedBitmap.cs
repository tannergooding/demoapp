// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace BitmapRendering.UnitTests;

/// <summary>A <see cref="Bitmap" /> backed by native buffers padded with sentinel guard cells.</summary>
/// <remarks>
/// The render buffer is allocated as [pad][body][pad] and every cell is seeded with a sentinel.
/// Any write outside the intended body -- for example the off-by-one entry pixel a clipped line
/// used to emit -- lands in a guard region, so <see cref="AssertGuardsIntact" /> catches it in any
/// build configuration, not just when the <c>Debug</c>-only bounds assert happens to fire.
/// </remarks>
internal sealed unsafe class GuardedBitmap : IDisposable
{
    private const uint Sentinel = 0xDEADBEEF;

    private readonly uint* _render;
    private readonly float* _depth;
    private readonly int _pad;
    private readonly int _count;

    public GuardedBitmap(int width, int height, int pad = 1024)
    {
        Width = width;
        Height = height;

        _pad = pad;
        _count = width * height;

        var total = (nuint)(pad + _count + pad);
        _render = (uint*)NativeMemory.Alloc(total, sizeof(uint));
        _depth = (float*)NativeMemory.Alloc(total, sizeof(float));

        for (nuint i = 0; i < total; i++)
        {
            _render[i] = Sentinel;
            _depth[i] = float.NaN;
        }

        Bitmap = new Bitmap((IntPtr)(_render + pad), (IntPtr)(_depth + pad), width, height);
    }

    public Bitmap Bitmap { get; }

    public int Width { get; }

    public int Height { get; }

    public IntPtr RenderBuffer => (IntPtr)(_render + _pad);

    public IntPtr DepthBuffer => (IntPtr)(_depth + _pad);

    public uint GetPixel(int x, int y) => _render[_pad + (y * Width) + x];

    public void AssertGuardsIntact()
    {
        for (var i = 0; i < _pad; i++)
        {
            Assert.That(_render[i], Is.EqualTo(Sentinel), "A pixel was written before the render buffer");
            Assert.That(_render[_pad + _count + i], Is.EqualTo(Sentinel), "A pixel was written past the render buffer");
        }
    }

    public void Dispose()
    {
        NativeMemory.Free(_render);
        NativeMemory.Free(_depth);
    }
}
