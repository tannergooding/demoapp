// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace Mathematics;

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
        var inverseProjection = projection.Invert();

        var points = stackalloc Vector4[] {
            s_homogenousPoints[0].Transform(inverseProjection),
            s_homogenousPoints[1].Transform(inverseProjection),
            s_homogenousPoints[2].Transform(inverseProjection),
            s_homogenousPoints[3].Transform(inverseProjection),
            s_homogenousPoints[4].Transform(inverseProjection),
            s_homogenousPoints[5].Transform(inverseProjection),
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
            _origin.Transform(transform.Rotation) + transform.Translation,
            _orientation.Transform(transform.Rotation),
            _rightSlope,
            _leftSlope,
            _topSlope,
            _bottomSlope,
            _near,
            _far
        );
    }
}
