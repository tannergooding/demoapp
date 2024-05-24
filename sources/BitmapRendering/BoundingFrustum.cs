// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Numerics;

namespace BitmapRendering;

public readonly struct BoundingFrustum(Vector3 origin, Vector4 orientation, float rightSlope, float leftSlope, float topSlope, float bottomSlope, float near, float far)
{
    private static readonly Vector4[] s_homogenousPoints = [
        new Vector4( 1.0f,  0.0f, 1.0f, 1.0f),
        new Vector4(-1.0f,  0.0f, 1.0f, 1.0f),
        new Vector4( 0.0f,  1.0f, 1.0f, 1.0f),
        new Vector4( 0.0f, -1.0f, 1.0f, 1.0f),
        new Vector4( 0.0f,  0.0f, 0.0f, 1.0f),
        new Vector4( 0.0f,  0.0f, 1.0f, 1.0f),
    ];

    private readonly Vector3 _origin = origin;
    private readonly Vector4 _orientation = orientation;
    private readonly float _rightSlope = rightSlope;
    private readonly float _leftSlope = leftSlope;
    private readonly float _topSlope = topSlope;
    private readonly float _bottomSlope = bottomSlope;
    private readonly float _near = near;
    private readonly float _far = far;

    public static unsafe BoundingFrustum CreateFrom(Matrix4x4 projection)
    {
        _ = Matrix4x4.Invert(projection, out var inverseProjection);

        var points = stackalloc Vector4[] {
            Vector4.Transform(s_homogenousPoints[0], inverseProjection),
            Vector4.Transform(s_homogenousPoints[1], inverseProjection),
            Vector4.Transform(s_homogenousPoints[2], inverseProjection),
            Vector4.Transform(s_homogenousPoints[3], inverseProjection),
            Vector4.Transform(s_homogenousPoints[4], inverseProjection),
            Vector4.Transform(s_homogenousPoints[5], inverseProjection),
        };

        return new BoundingFrustum(
            Vector3.Zero,
            Vector4.UnitW,
            (points[0] / points[0].Z).X,
            (points[1] / points[1].Z).X,
            (points[2] / points[2].Z).Y,
            (points[3] / points[3].Z).Y,
            (points[4] / points[4].W).Z,
            (points[5] / points[5].W).Z
        );
    }

    public BoundingFrustum Transform(OrthogonalTransform transform)
    {
        return new BoundingFrustum(
            Vector3.Transform(_origin, transform.Rotation) + transform.Translation,
            Vector4.Transform(_orientation, transform.Rotation),
            _rightSlope,
            _leftSlope,
            _topSlope,
            _bottomSlope,
            _near,
            _far
        );
    }
}
