namespace Mathematics
{
    public readonly struct BoundingFrustum
    {
        #region Default Instances
        private static readonly Vector4[] HomogenousPoints = new Vector4[] {
            new Vector4( 1.0f,  0.0f, 1.0f, 1.0f),
            new Vector4(-1.0f,  0.0f, 1.0f, 1.0f),
            new Vector4( 0.0f,  1.0f, 1.0f, 1.0f),
            new Vector4( 0.0f, -1.0f, 1.0f, 1.0f),
            new Vector4( 0.0f,  0.0f, 0.0f, 1.0f),
            new Vector4( 0.0f,  0.0f, 1.0f, 1.0f),
        };
        #endregion

        #region Fields
        private readonly Vector3 Origin;
        private readonly Vector4 Orientation;
        private readonly float RightSlope;
        private readonly float LeftSlope;
        private readonly float TopSlope;
        private readonly float BottomSlope;
        private readonly float Near;
        private readonly float Far;
        #endregion

        #region Constructors
        public BoundingFrustum(Vector3 origin, Vector4 orientation, float rightSlope, float leftSlope, float topSlope, float bottomSlope, float near, float far)
        {
            Origin = origin;
            Orientation = orientation;
            RightSlope = rightSlope;
            LeftSlope = leftSlope;
            TopSlope = topSlope;
            BottomSlope = bottomSlope;
            Near = near;
            Far = far;
        }
        #endregion

        #region Methods
        public static unsafe BoundingFrustum CreateFrom(Matrix4x4 projection)
        {
            var inverseProjection = projection.Invert();

            var points = stackalloc Vector4[] {
                HomogenousPoints[0].Transform(inverseProjection),
                HomogenousPoints[1].Transform(inverseProjection),
                HomogenousPoints[2].Transform(inverseProjection),
                HomogenousPoints[3].Transform(inverseProjection),
                HomogenousPoints[4].Transform(inverseProjection),
                HomogenousPoints[5].Transform(inverseProjection),
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
                Origin.Transform(transform.Rotation) + transform.Translation,
                Orientation.Transform(transform.Rotation),
                RightSlope,
                LeftSlope,
                TopSlope,
                BottomSlope,
                Near,
                Far
            );
        }
        #endregion
    }
}
