// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Numerics;

namespace BitmapRendering;

public readonly struct OrthogonalTransform(Quaternion rotation, Vector3 translation)
{
    public static readonly OrthogonalTransform Identity = new OrthogonalTransform(Quaternion.Identity, Vector3.Zero);

    public readonly Quaternion Rotation = rotation;
    public readonly Vector3 Translation = translation;

    public OrthogonalTransform Invert()
    {
        var inverseRotation = Quaternion.Conjugate(Rotation);
        return new OrthogonalTransform(inverseRotation, -Vector3.Transform(Translation, inverseRotation));
    }

    public OrthogonalTransform WithRotation(Quaternion rotation) => new OrthogonalTransform(rotation, Translation);

    public OrthogonalTransform WithTranslation(Vector3 translation) => new OrthogonalTransform(Rotation, translation);
}
