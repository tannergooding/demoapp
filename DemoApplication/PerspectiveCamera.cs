using System;
using System.Collections.Generic;
using System.Text;
using Mathematics;

namespace DemoApplication
{
    public class PerspectiveCamera
    {
        private OrthogonalTransform _cameraToWorld;
        private Matrix3x3 _basis = Matrix3x3.Identity;
        private Matrix4x4 _view = Matrix4x4.Identity;
        private Matrix4x4 _projection = Matrix4x4.Identity;
        private Matrix4x4 _viewProjection = Matrix4x4.Identity;
        private Matrix4x4 _previousViewProjection = Matrix4x4.Identity;
        private Matrix4x4 _reprojection = Matrix4x4.Identity;

        private BoundingFrustum _viewSpace = new BoundingFrustum();
        private BoundingFrustum _worldSpace = new BoundingFrustum();

        private float _fieldOfView = MathF.PI / 4.0f;
        private float _aspectRatio = 9.0f / 16.0f;
        private float _nearClip = 1.0f;
        private float _farClip = 1000.0f;
        private bool _reverseZ = true;

        public PerspectiveCamera()
        {
            SetPerspective(MathF.PI / 4.0f, 9.0f / 16.0f, 1.0f, 1000.0f);
        }

        #region Properties
        public float AspectRatio
        {
            get
            {
                return _aspectRatio;
            }

            set
            {
                _aspectRatio = value;
            }
        }

        public float ClearDepth
        {
            get
            {
                return _reverseZ ? 0.0f : 1.0f;
            }
        }

        public float FarClip
        {
            get
            {
                return _farClip;
            }
        }

        public float FieldOfView
        {
            get
            {
                return _fieldOfView;
            }

            set
            {
                _fieldOfView = value;
            }
        }

        public Vector3 Forward
        {
            get
            {
                return -_basis.Z;
            }
        }

        public float NearClip
        {
            get
            {
                return _nearClip;
            }
        }

        public Vector3 Position
        {
            get
            {
                return _cameraToWorld.Translation;
            }

            set
            {
                _cameraToWorld = _cameraToWorld.WithTranslation(value);
            }
        }

        public Matrix4x4 Projection
        {
            get
            {
                return _projection;
            }

            set
            {
                _projection = value;
            }
        }

        public Matrix4x4 Reprojection
        {
            get
            {
                return _reprojection;
            }
        }

        public bool ReverseZ
        {
            get
            {
                return _reverseZ;
            }

            set
            {
                _reverseZ = value;
            }
        }

        public Vector3 Right
        {
            get
            {
                return _basis.X;
            }
        }

        public Quaternion Rotation
        {
            get
            {
                return _cameraToWorld.Rotation;
            }

            set
            {
                _cameraToWorld = _cameraToWorld.WithRotation(value.Normalize());
                _basis = Matrix3x3.CreateFrom(_cameraToWorld.Rotation);
            }
        }

        public Vector3 Up
        {
            get
            {
                return _basis.Y;
            }
        }

        public Matrix4x4 View
        {
            get
            {
                return _view;
            }
        }

        public Matrix4x4 ViewProjection
        {
            get
            {
                return _viewProjection;
            }
        }

        public BoundingFrustum ViewSpace
        {
            get
            {
                return _viewSpace;
            }
        }

        public BoundingFrustum WorldSpace
        {
            get
            {
                return _worldSpace;
            }
        }
        #endregion

        #region Methods
        public void SetClip(float nearClip, float farClip)
        {
            _nearClip = nearClip;
            _farClip = farClip;
        }

        public void SetEyeAtUp(Vector3 eye, Vector3 at, Vector3 up)
        {
            SetLookDirection((at - eye), up);
            Position = eye;
        }

        public void SetLookDirection(Vector3 forward, Vector3 up)
        {
            var forwardLengthSq = forward.LengthSquared;
            forward = (forwardLengthSq < 0.000001f) ? -Vector3.UnitZ : forward / MathF.Sqrt(forwardLengthSq);

            var right = Vector3.CrossProduct(forward, up);
            var rightLengthSq = right.LengthSquared;
            right = (rightLengthSq < 0.000001f)
                  ? forward.Transform(new Quaternion(Vector3.UnitY, -MathF.PI / 2.0f))
                  : right / MathF.Sqrt(rightLengthSq);

            up = Vector3.CrossProduct(right, forward);

            _basis = new Matrix3x3(right, up, -forward);
            _cameraToWorld = _cameraToWorld.WithRotation(Quaternion.CreateFrom(_basis));
        }

        public void SetPerspective(float fieldOfView, float aspectRatio, float nearClip, float farClip)
        {
            _fieldOfView = fieldOfView;
            _aspectRatio = aspectRatio;

            _nearClip = nearClip;
            _farClip = farClip;

            UpdateProjection();

            _previousViewProjection = _viewProjection;
        }

        public void Update()
        {
            _previousViewProjection = _viewProjection;

            _view = Matrix4x4.CreateFrom(_cameraToWorld.Invert());
            _viewProjection = _projection * _view;
            _reprojection = _previousViewProjection * _viewProjection.Invert();

            _viewSpace = BoundingFrustum.CreateFrom(_projection);
            _worldSpace = _viewSpace.Transform(_cameraToWorld);
        }

        private void UpdateProjection()
        {
            var y = 1.0f / MathF.Tan(_fieldOfView * 0.5f);
            var x = y * _aspectRatio;

            float q1, q2;

            if (_reverseZ)
            {
                var temp = _nearClip / (_farClip - _nearClip);
                q1 = temp;
                q2 = temp * _farClip;
            }
            else
            {
                var temp = _farClip / (_nearClip - _farClip);
                q1 = temp;
                q2 = temp * _nearClip;
            }

            var projection = new Matrix4x4(new Vector4(x, 0.0f, 0.0f, 0.0f),
                                           new Vector4(0.0f, y, 0.0f, 0.0f),
                                           new Vector4(0.0f, 0.0f, q1, -1.0f),
                                           new Vector4(0.0f, 0.0f, q2, 0.0f));

            _projection = projection;
        }
        #endregion
    }
}
