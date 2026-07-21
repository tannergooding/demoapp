// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Numerics;
using NUnit.Framework;

namespace BitmapRendering.UnitTests;

/// <summary>Provides a set of tests covering the <see cref="Bitmap" /> rasterizer.</summary>
internal static class BitmapTests
{
    private const uint Color = 0xFFFFFFFF;

    // The screen-space guard band the projection clamps to; DrawLine2D must stay
    // overflow- and bounds-safe for any endpoints within it.
    private const int CoordinateLimit = 1 << 14;

    /// <summary>Validates that a fully on-screen line writes its start pixel and nothing outside the buffer.</summary>
    [Test]
    public static void DrawLine2DDrawsStartPixelWhenOnScreen()
    {
        using var bitmap = new GuardedBitmap(800, 600);

        bitmap.Bitmap.DrawLine2D(new Vector3(100.0f, 100.0f, 0.0f), new Vector3(200.0f, 150.0f, 0.0f), Color, useHWIntrinsics: false);

        Assert.That(bitmap.GetPixel(100, 100), Is.EqualTo(Color));
        bitmap.AssertGuardsIntact();
    }

    /// <summary>Validates that a clipped line from an on-screen point to a far off-screen point never writes out of bounds.</summary>
    /// <remarks>These are the exact endpoints traced back from the near-plane projection that used to write the entry pixel one step outside the window.</remarks>
    [TestCase(605, 454, -4075, -3056)]
    [TestCase(175, 382, 6425, -3443)]
    [TestCase(623, 352, 8250, -14902)]
    [TestCase(213, 510, -214, -512)]
    [TestCase(781, 586, -16031, -12023)]
    [TestCase(704, 493, -15716, -11001)]
    public static void DrawLine2DStaysInBoundsForKnownNearPlaneLines(int x1, int y1, int x2, int y2)
    {
        using var bitmap = new GuardedBitmap(800, 600);

        Assert.DoesNotThrow(() => bitmap.Bitmap.DrawLine2D(new Vector3(x1, y1, 0.0f), new Vector3(x2, y2, 0.0f), Color, useHWIntrinsics: false));
        bitmap.AssertGuardsIntact();
    }

    /// <summary>Validates that lines from the center to every direction and distance within the guard band stay in bounds.</summary>
    [Test]
    public static void DrawLine2DStaysInBoundsForAllOffscreenDirections()
    {
        using var bitmap = new GuardedBitmap(800, 600);
        var origin = new Vector3(400.0f, 300.0f, 0.0f);

        for (var degrees = 0; degrees < 360; degrees++)
        {
            var radians = degrees * (MathF.PI / 180.0f);
            var direction = new Vector2(MathF.Cos(radians), MathF.Sin(radians));

            foreach (var radius in new[] { 1000.0f, 8000.0f, CoordinateLimit - 1.0f })
            {
                var target = new Vector3(400.0f + (direction.X * radius), 300.0f + (direction.Y * radius), 0.0f);
                bitmap.Bitmap.DrawLine2D(origin, target, Color, useHWIntrinsics: false);
            }
        }

        bitmap.AssertGuardsIntact();
    }

    /// <summary>Validates that <see cref="Bitmap.DrawPixel" /> ignores coordinates outside the viewport.</summary>
    [Test]
    public static void DrawPixelIsNoOpWhenOutOfBounds()
    {
        using var bitmap = new GuardedBitmap(800, 600);

        Assert.DoesNotThrow(() => bitmap.Bitmap.DrawPixel(new Vector3(-5.0f, -5.0f, 0.0f), Color));
        Assert.DoesNotThrow(() => bitmap.Bitmap.DrawPixel(new Vector3(10000.0f, 10000.0f, 0.0f), Color));
        bitmap.AssertGuardsIntact();
    }
}
