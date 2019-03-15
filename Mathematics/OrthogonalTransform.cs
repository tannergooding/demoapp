namespace Mathematics
{
    public readonly struct OrthogonalTransform
    {
        #region Default Instances
        private static readonly OrthogonalTransform Identity = new OrthogonalTransform(Quaternion.Identity, Vector3.Zero);
        #endregion

        #region Fields
        public readonly Quaternion Rotation;
        public readonly Vector3 Translation;
        #endregion

        #region Constructors
        public OrthogonalTransform(Quaternion rotation, Vector3 translation)
        {
            Rotation = rotation;
            Translation = translation;
        }
        #endregion

        #region Methods
        public OrthogonalTransform Invert()
        {
            var inverseRotation = Rotation.Conjugate;
            return new OrthogonalTransform(inverseRotation, -Translation.Transform(inverseRotation));
        }

        public OrthogonalTransform WithRotation(Quaternion rotation)
        {
            return new OrthogonalTransform(rotation, Translation);
        }

        public OrthogonalTransform WithTranslation(Vector3 translation)
        {
            return new OrthogonalTransform(Rotation, translation);
        }
        #endregion
    }
}
