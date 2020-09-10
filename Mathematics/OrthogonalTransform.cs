// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace Mathematics
{
    public readonly struct OrthogonalTransform
    {
        private static readonly OrthogonalTransform Identity = new OrthogonalTransform(Quaternion.Identity, Vector3.Zero);

        public readonly Quaternion Rotation;
        public readonly Vector3 Translation;

        public OrthogonalTransform(Quaternion rotation, Vector3 translation)
        {
            Rotation = rotation;
            Translation = translation;
        }

        public OrthogonalTransform Invert()
        {
            var inverseRotation = Rotation.Conjugate;
            return new OrthogonalTransform(inverseRotation, -Translation.Transform(inverseRotation));
        }

        public OrthogonalTransform WithRotation(Quaternion rotation) => new OrthogonalTransform(rotation, Translation);

        public OrthogonalTransform WithTranslation(Vector3 translation) => new OrthogonalTransform(Rotation, translation);
    }
}
