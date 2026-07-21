// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Numerics;
using NUnit.Framework;

namespace BitmapRendering.UnitTests;

/// <summary>Provides a set of tests covering the <see cref="BitmapRenderer" /> projection and rasterization pipeline.</summary>
internal static class BitmapRendererTests
{
    private const float DegreesToRadians = MathF.PI / 180.0f;

    private static readonly float[] s_sweepAngles = new[] { 0.0f, 45.0f, 90.0f, 135.0f, 180.0f, 225.0f, 270.0f, 315.0f };

    /// <summary>Validates that projecting and rasterizing geometry never writes out of bounds, across orientations and zoom.</summary>
    /// <remarks>
    /// At high zoom the perspective divide pushes cube corners onto or behind the near plane, which previously
    /// overflowed the checked integer rasterizer math and could write outside the buffer. Sweeping orientation
    /// catches angle-specific crossings; the guarded buffer catches any stray write in any configuration.
    /// </remarks>
    [Test]
    public static void RenderStaysInBoundsAcrossOrientationsAndZoom()
    {
        using var bitmap = new GuardedBitmap(800, 600);

        foreach (var isWireframe in new[] { true, false })
        {
            var zooms = isWireframe ? new[] { 400.0f, 1600.0f, 3200.0f } : new[] { 200.0f, 800.0f };

            foreach (var zoom in zooms)
            {
                foreach (var pitch in s_sweepAngles)
                {
                    foreach (var yaw in s_sweepAngles)
                    {
                        var rotation = Quaternion.CreateFromYawPitchRoll(yaw * DegreesToRadians, pitch * DegreesToRadians, 0.0f);

                        var renderer = new BitmapRenderer();
                        renderer.Reset();
                        renderer.Wireframe = isWireframe;
                        renderer.ZoomLevel = zoom;
                        renderer.ActiveScene = CreateCube(rotation);

                        renderer.Update(bitmap.RenderBuffer, bitmap.DepthBuffer, bitmap.Width, bitmap.Height);
                        renderer.Render();
                    }
                }
            }
        }

        bitmap.AssertGuardsIntact();
    }

    /// <summary>Validates that a model at the default zoom actually rasterizes over the cleared background.</summary>
    [Test]
    public static void RenderDrawsGeometryAtDefaultZoom()
    {
        using var bitmap = new GuardedBitmap(800, 600);

        var renderer = new BitmapRenderer();
        renderer.Reset();
        renderer.Wireframe = true;
        renderer.ActiveScene = CreateCube(Quaternion.Identity);

        renderer.Update(bitmap.RenderBuffer, bitmap.DepthBuffer, bitmap.Width, bitmap.Height);
        renderer.Render();

        var background = bitmap.GetPixel(0, 0);
        var drewGeometry = false;

        for (var y = 0; (y < bitmap.Height) && !drewGeometry; y++)
        {
            for (var x = 0; x < bitmap.Width; x++)
            {
                if (bitmap.GetPixel(x, y) != background)
                {
                    drewGeometry = true;
                    break;
                }
            }
        }

        Assert.That(drewGeometry, Is.True);
        bitmap.AssertGuardsIntact();
    }

    // A unit cube centered at the origin, pre-rotated so orientation can be swept without touching renderer internals.
    private static Model CreateCube(Quaternion rotation)
    {
        var model = new Model(verticeCount: 8, verticeGroupCount: 6, normalCount: 6, normalGroupCount: 6);

        var corners = new[]
        {
            new Vector3(-1.0f, -1.0f, -1.0f),
            new Vector3(+1.0f, -1.0f, -1.0f),
            new Vector3(+1.0f, +1.0f, -1.0f),
            new Vector3(-1.0f, +1.0f, -1.0f),
            new Vector3(-1.0f, -1.0f, +1.0f),
            new Vector3(+1.0f, -1.0f, +1.0f),
            new Vector3(+1.0f, +1.0f, +1.0f),
            new Vector3(-1.0f, +1.0f, +1.0f),
        };

        foreach (var corner in corners)
        {
            model.Vertices.Add(Vector3.Transform(corner, rotation));
        }

        model.VerticeGroups.Add(new[] { 0, 1, 2, 3 });
        model.VerticeGroups.Add(new[] { 4, 5, 6, 7 });
        model.VerticeGroups.Add(new[] { 0, 3, 7, 4 });
        model.VerticeGroups.Add(new[] { 1, 5, 6, 2 });
        model.VerticeGroups.Add(new[] { 0, 4, 5, 1 });
        model.VerticeGroups.Add(new[] { 3, 2, 6, 7 });

        var faceNormals = new[]
        {
            new Vector3(0.0f, 0.0f, -1.0f),
            new Vector3(0.0f, 0.0f, +1.0f),
            new Vector3(-1.0f, 0.0f, 0.0f),
            new Vector3(+1.0f, 0.0f, 0.0f),
            new Vector3(0.0f, -1.0f, 0.0f),
            new Vector3(0.0f, +1.0f, 0.0f),
        };

        for (var i = 0; i < faceNormals.Length; i++)
        {
            model.Normals.Add(Vector3.Transform(faceNormals[i], rotation));
            model.NormalGroups.Add(new[] { i });
        }

        return model;
    }
}
