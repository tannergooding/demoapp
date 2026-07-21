// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using System.Numerics;

namespace BitmapRendering;

public sealed class BitmapRenderer
{
    private const float DefaultLightPositionX = 0.0f;
    private const float DefaultLightPositionY = 0.0f;
    private const float DefaultLightPositionZ = 1000.0f;
    private const float DefaultRotationXSpeed = -5.0f;
    private const float DefaultRotationYSpeed = 0.0f;
    private const float DefaultRotationZSpeed = 5.0f;
    private const float DefaultZoomLevel = 100.0f;
    private const float TicksPerSecond = TimeSpan.TicksPerSecond;

    private const float CameraDistance = 8.0f;
    private const float NearClip = 0.1f;
    private const float FarClip = 100.0f;
    private const float ZoomToWorldScale = 0.01f;

    private static readonly Vector3 s_defaultRotation = new Vector3(90.0f, 0.0f, 0.0f);

    private static readonly Vector3 s_defaultScale = new Vector3(DefaultZoomLevel, DefaultZoomLevel, 1.0f);
    private static readonly TimeSpan s_oneSecond = TimeSpan.FromSeconds(1.0);

    private Bitmap _bitmap;

    private int _minFps = int.MaxValue;
    private int _fps;
    private int _maxFps = int.MinValue;
    private long _totalFrames;

    private long _previousTimestamp = Stopwatch.GetTimestamp();
    private TimeSpan _totalUptime = TimeSpan.Zero;
    private TimeSpan _lastHeaderUpdate = TimeSpan.Zero;

    private Vector3 _lightPosition = Vector3.Zero;
    private Vector3 _modifiedLightPosition = Vector3.Zero;
    private Vector3 _rotation = Vector3.Zero;
    private Vector3 _rotationSpeed = Vector3.Zero;
    private Vector3 _scale = Vector3.One;

    private readonly PerspectiveCamera _camera = new PerspectiveCamera();

    private readonly uint _backgroundColor = 0xFF6495ED; // Cornflower Blue
    private readonly uint _foregroundColor = 0xFFFFFFFF; // White

    private bool _isRotating = true;
    private bool _isWireframe = true;
    private bool _useHWIntrinsics;
    private bool _displayDepthBuffer;

    public BitmapRenderer()
    {
        Reset();
        _previousTimestamp = Stopwatch.GetTimestamp();
    }

    public Model? ActiveScene { get; set; }

    public bool DisplayDepthBuffer
    {
        get
        {
            return _displayDepthBuffer;
        }

        set
        {
            _displayDepthBuffer = value;
        }
    }

    public float LightPositionX
    {
        get
        {
            return _lightPosition.X;
        }

        set
        {
            _lightPosition.X = value;
        }
    }

    public float LightPositionY
    {
        get
        {
            return _lightPosition.Y;
        }

        set
        {
            _lightPosition.Y = value;
        }
    }

    public float LightPositionZ
    {
        get
        {
            return _lightPosition.Z;
        }

        set
        {
            _lightPosition.Z = value;
        }
    }

    public bool RotateModel
    {
        get
        {
            return _isRotating;
        }

        set
        {
            _isRotating = value;
        }
    }

    public float RotationXSpeed
    {
        get
        {
            return _rotationSpeed.X;
        }

        set
        {
            _rotationSpeed.X = value;
        }
    }

    public float RotationYSpeed
    {
        get
        {
            return _rotationSpeed.Y;
        }

        set
        {
            _rotationSpeed.Y = value;
        }
    }

    public float RotationZSpeed
    {
        get
        {
            return _rotationSpeed.Z;
        }

        set
        {
            _rotationSpeed.Z = value;
        }
    }

    public string Title { get; private set; } = "";

    public bool UseHWIntrinsics
    {
        get
        {
            return _useHWIntrinsics;
        }

        set
        {
            _useHWIntrinsics = value;
        }
    }

    public bool Wireframe
    {
        get
        {
            return _isWireframe;
        }

        set
        {
            _isWireframe = value;
        }
    }

    public float ZoomLevel
    {
        get
        {
            return _scale.X;
        }

        set
        {
            _scale = new Vector3(value, value, 1.0f);
        }
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

        _rotation = s_defaultRotation;
        _scale = s_defaultScale;
    }

    public void Update(IntPtr renderBuffer, IntPtr depthBuffer, int pixelWidth, int pixelHeight)
    {
        _bitmap = new Bitmap(renderBuffer, depthBuffer, pixelWidth, pixelHeight);

        var timestamp = Stopwatch.GetTimestamp();
        var delta = Stopwatch.GetElapsedTime(_previousTimestamp, timestamp);

        _totalUptime += delta;
        _lastHeaderUpdate += delta;

        UpdateRotation(delta);

        var activeScene = ActiveScene;

        if (activeScene != null)
        {
            _modifiedLightPosition = _lightPosition;
            activeScene.EnsureInitialized();

            ProjectScene(activeScene, pixelWidth, pixelHeight);
        }

        _previousTimestamp = timestamp;
    }

    private void ProjectScene(Model model, int pixelWidth, int pixelHeight)
    {
        var rotationRadians = _rotation * (MathF.PI / 180.0f);
        var rotation = Quaternion.CreateFromYawPitchRoll(rotationRadians.Y, rotationRadians.X, rotationRadians.Z);
        var worldScale = ZoomLevel * ZoomToWorldScale;

        _camera.SetPerspective(_camera.FieldOfView, (float)pixelHeight / pixelWidth, NearClip, FarClip);
        _camera.SetEyeAtUp(new Vector3(0.0f, 0.0f, CameraDistance), Vector3.Zero, Vector3.UnitY);
        _camera.Update();

        var modelToClip = Matrix4x4.CreateScale(worldScale) * Matrix4x4.CreateFromQuaternion(rotation) * _camera.ViewProjection;

        var sourceVertices = model.Vertices;
        var vertices = model.ModifiedVertices;

        for (var i = 0; i < sourceVertices.Count; i++)
        {
            var clip = Vector4.Transform(new Vector4(sourceVertices[i], 1.0f), modelToClip);
            var ndc = new Vector3(clip.X, clip.Y, clip.Z) / clip.W;

            vertices[i] = new Vector3(
                ((ndc.X * 0.5f) + 0.5f) * pixelWidth,
                (1.0f - ((ndc.Y * 0.5f) + 0.5f)) * pixelHeight,
                ndc.Z
            );
        }

        var sourceNormals = model.Normals;
        var normals = model.ModifiedNormals;

        for (var i = 0; i < sourceNormals.Count; i++)
        {
            normals[i] = Vector3.Transform(sourceNormals[i], rotation);
        }
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

        _lastHeaderUpdate -= s_oneSecond;
        _fps = 0;
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
}
