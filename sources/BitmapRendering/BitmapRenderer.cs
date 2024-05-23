// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using Mathematics;

namespace BitmapRendering
{
    public sealed class BitmapRenderer
    {
        private const float DefaultLightPositionX = 0.0f;
        private const float DefaultLightPositionY = 0.0f;
        private const float DefaultLightPositionZ = 1000.0f;
        private const float DefaultRotationXSpeed = -5.0f;
        private const float DefaultRotationYSpeed = 0.0f;
        private const float DefaultRotationZSpeed = 5.0f;
        private const float DefaultZoomLevel = 100.0f;
        private const float TicksPerSecond = TimeSpan.TicksPerSecond; private static readonly Vector3 DefaultRotation = new Vector3(90.0f, 0.0f, 0.0f);

        private static readonly Vector3 DefaultScale = new Vector3(DefaultZoomLevel, DefaultZoomLevel, 1.0f);
        private static readonly TimeSpan OneSecond = TimeSpan.FromSeconds(1.0);
        private static readonly float TickFrequency = TicksPerSecond / Stopwatch.Frequency;

        private Bitmap _bitmap;

        private int _minFps = int.MaxValue;
        private int _fps = 0;
        private int _maxFps = int.MinValue;
        private long _totalFrames = 0;

        private Timestamp _previousTimestamp = new Timestamp(0);
        private TimeSpan _totalUptime = TimeSpan.Zero;
        private TimeSpan _lastHeaderUpdate = TimeSpan.Zero;

        private Vector3 _lightPosition = Vector3.Zero;
        private Vector3 _modifiedLightPosition = Vector3.Zero;
        private Vector3 _rotation = Vector3.Zero;
        private Vector3 _rotationSpeed = Vector3.Zero;
        private Vector3 _scale = Vector3.One;
        private Vector3 _translation = Vector3.Zero;

        private PerspectiveCamera _camera = new PerspectiveCamera();

        private uint _backgroundColor = 0xFF6495ED; // Cornflower Blue
        private uint _foregroundColor = 0xFFFFFFFF; // White

        private bool _isRotating = true;
        private bool _isWireframe = true;
        private bool _useHWIntrinsics = false;
        private bool _displayDepthBuffer = false;

        public BitmapRenderer()
        {
            Reset();
            _camera.SetEyeAtUp(Vector3.Zero, Vector3.Zero, Vector3.UnitY);
            _previousTimestamp = GetTimestamp();
        }

        public Model? ActiveScene { get; set; }

        public bool DisplayDepthBuffer
        {
            get => _displayDepthBuffer;
            set => _displayDepthBuffer = value;
        }

        public float LightPositionX
        {
            get => _lightPosition.X;
            set => _lightPosition.X = value;
        }

        public float LightPositionY
        {
            get => _lightPosition.Y;
            set => _lightPosition.Y = value;
        }

        public float LightPositionZ
        {
            get => _lightPosition.Z;
            set => _lightPosition.Z = value;
        }

        public bool RotateModel
        {
            get => _isRotating;
            set => _isRotating = value;
        }

        public float RotationXSpeed
        {
            get => _rotationSpeed.X;
            set => _rotationSpeed.X = value;
        }

        public float RotationYSpeed
        {
            get => _rotationSpeed.Y;
            set => _rotationSpeed.Y = value;
        }

        public float RotationZSpeed
        {
            get => _rotationSpeed.Z;
            set => _rotationSpeed.Z = value;
        }

        public string Title { get; private set; } = "";

        public bool UseHWIntrinsics
        {
            get => _useHWIntrinsics;
            set => _useHWIntrinsics = value;
        }

        public bool Wireframe
        {
            get => _isWireframe;
            set => _isWireframe = value;
        }

        public float ZoomLevel
        {
            get => _scale.X;
            set => _scale = new Vector3(value, value, 1.0f);
        }

        public void Present()
        {
            _fps++;
            _totalFrames++;
        }

        public void Render()
        {
            RenderInfo();
            RenderBuffer();
        }

        public void Reset()
        {
            RotationXSpeed = DefaultRotationXSpeed;
            RotationYSpeed = DefaultRotationYSpeed;
            RotationZSpeed = DefaultRotationZSpeed;

            LightPositionX = DefaultLightPositionX;
            LightPositionY = DefaultLightPositionY;
            LightPositionZ = DefaultLightPositionZ;

            ZoomLevel = DefaultZoomLevel;

            DisplayDepthBuffer = false;
            RotateModel = false;
            UseHWIntrinsics = false;
            Wireframe = true;

            _rotation = DefaultRotation;
            _scale = DefaultScale;
        }

        public void Update(IntPtr renderBuffer, IntPtr depthBuffer, int pixelWidth, int pixelHeight)
        {
            _bitmap = new Bitmap(renderBuffer, depthBuffer, pixelWidth, pixelHeight);

            var timestamp = GetTimestamp();
            var delta = timestamp - _previousTimestamp;

            _totalUptime += delta;
            _lastHeaderUpdate += delta;

            _translation = new Vector3(pixelWidth / 2.0f, pixelHeight / 2.0f, 0.0f);

            UpdateRotation(delta);

            var activeScene = ActiveScene;

            if (activeScene != null)
            {
                _modifiedLightPosition = _lightPosition;
                activeScene.Reset();

                ObjectToWorld(activeScene);
                WorldToCamera(activeScene);
                CameraToScreen(activeScene);
            }

            _previousTimestamp = timestamp;
        }

        private static Timestamp GetTimestamp()
        {
            double ticks = Stopwatch.GetTimestamp();
            ticks *= TickFrequency;
            return new Timestamp((long)ticks);
        }

        private void CameraToScreen(Model model) { }

        private void ObjectToWorld(Model model)
        {
            RotateObject(model);
            ScaleObject(model);
            TranslateObject(model);
        }

        private void RenderBuffer()
        {
            _bitmap.Clear(_backgroundColor, _camera.ClearDepth, _useHWIntrinsics);

            if (ActiveScene != null)
            {
                _bitmap.DrawModel(ActiveScene, _modifiedLightPosition, _foregroundColor, _isWireframe, _useHWIntrinsics);
            }
        }

        private void RenderInfo()
        {
            if (_lastHeaderUpdate.Ticks < TimeSpan.TicksPerSecond)
            {
                return;
            }

            _minFps = Math.Min(_minFps, _fps);
            _maxFps = Math.Max(_maxFps, _fps);

            Title = $"FPS: {_fps}; Min FPS: {_minFps}; Max FPS: {_maxFps}; Avg FPS: {_totalFrames / (_totalUptime.Ticks / TicksPerSecond):F2}; Resolution: {_bitmap.PixelWidth}x{_bitmap.PixelHeight}; Vertices: {((ActiveScene is null) ? 0 : ActiveScene.ModifiedVertices.Count)}";

            _lastHeaderUpdate -= OneSecond;
            _fps = 0;
        }

        private void RotateObject(Model polygon)
        {
            var rotation = _rotation * (MathF.PI / 180);
            var rotationTransform = Quaternion.CreateFrom(rotation.X, rotation.Y, rotation.Z);

            for (var i = 0; i < polygon.Vertices.Count; i++)
            {
                polygon.ModifiedVertices[i] = polygon.ModifiedVertices[i].Transform(rotationTransform);
            };

            for (var i = 0; i < polygon.Normals.Count; i++)
            {
                polygon.ModifiedNormals[i] = polygon.ModifiedNormals[i].Transform(rotationTransform);
            }
        }

        private void ScaleObject(Model polygon)
        {
            var scale = _scale;

            var scaleTransform = new Matrix3x3(new Vector3(scale.X, 0.0f, 0.0f),
                                               new Vector3(0.0f, scale.Y, 0.0f),
                                               new Vector3(0.0f, 0.0f, scale.Z));

            for (var i = 0; i < polygon.Vertices.Count; i++)
            {
                polygon.ModifiedVertices[i] = polygon.ModifiedVertices[i].Transform(scaleTransform);
            }
        }

        private void TranslateObject(Model polygon)
        {
            var translation = _translation;
            _modifiedLightPosition += translation;

            for (var i = 0; i < polygon.Vertices.Count; i++)
            {
                polygon.ModifiedVertices[i] += translation;
            }
        }

        private void UpdateRotation(TimeSpan delta)
        {
            if (!_isRotating)
            {
                return;
            }

            var rotation = _rotation;
            rotation += _rotationSpeed * (delta.Ticks / TicksPerSecond);

            if (rotation.X >= 360.0f)
            {
                rotation.X -= 360.0f;
            }

            if (rotation.Y >= 360.0f)
            {
                rotation.Y -= 360.0f;
            }

            if (rotation.Z >= 360.0f)
            {
                rotation.Z -= 360.0f;
            }

            _rotation = rotation;
        }

        private void WorldToCamera(Model polygon)
        {
            var viewProjection = _camera.ViewProjection;
            _modifiedLightPosition = _modifiedLightPosition.Transform(viewProjection);

            for (var i = 0; i < polygon.Vertices.Count; i++)
            {
                polygon.ModifiedVertices[i] = polygon.ModifiedVertices[i].Transform(viewProjection);
            }

            for (var i = 0; i < polygon.Normals.Count; i++)
            {
                polygon.ModifiedNormals[i] = polygon.ModifiedNormals[i].Transform(viewProjection);
            }
        }
    }
}
