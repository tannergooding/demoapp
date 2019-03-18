using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Mathematics;

namespace DemoApplication
{
    public partial class MainWindow : Window
    {
        #region Constants
        private const int BufferCount = 2;
        private const float DefaultRotationXSpeed = -5.0f;
        private const float DefaultRotationYSpeed = 0.0f;
        private const float DefaultRotationZSpeed = 5.0f;
        private const float DefaultZoomLevel = 100.0f;
        private const float TicksPerSecond = TimeSpan.TicksPerSecond;

        private static readonly Vector3 DefaultRotation = new Vector3(90.0f, 0.0f, 0.0f);
        private static readonly Vector3 DefaultScale = new Vector3(DefaultZoomLevel, DefaultZoomLevel, 1.0f);
        private static readonly TimeSpan OneSecond = TimeSpan.FromSeconds(1.0);
        private static readonly float TickFrequency = TicksPerSecond / Stopwatch.Frequency;
        #endregion

        #region Fields
        private readonly Bitmap[] _buffers = new Bitmap[BufferCount];

        private int _renderBufferIndex = 0;

        private int _minFps = int.MaxValue;
        private int _fps = 0;
        private int _maxFps = int.MinValue;
        private long _totalFrames = 0;

        private Timestamp _previousTimestamp = new Timestamp(0);
        private TimeSpan _totalUptime = TimeSpan.Zero;
        private TimeSpan _lastHeaderUpdate = TimeSpan.Zero;

        private Vector3 _rotation = Vector3.Zero;
        private Vector3 _rotationSpeed = Vector3.Zero;
        private Vector3 _scale = Vector3.Unit;
        private Vector3 _translation = Vector3.Zero;

        private PerspectiveCamera _camera = new PerspectiveCamera();

        private uint _backgroundColor = 0xFF6495ED; // Cornflower Blue
        private float _clearDepth = float.MinValue;
        private uint _foregroundColor = 0xFF000000; // Black

        private bool _isRotating = true;
        private bool _isWireframe = true;
        private bool _useHWIntrinsics = false;
        private bool _displayDepthBuffer = false;

        private readonly List<Model> _scenes = new List<Model>();
        private Model _activeScene = null;
        #endregion

        #region Constructors
        public MainWindow()
        {
            InitializeComponent();
            Startup();
        }
        #endregion

        #region Event Handlers
        private void OnApplicationIdle(object sender, EventArgs e)
        {
            var timestamp = GetTimestamp();
            {
                Update(timestamp - _previousTimestamp);
                Render();
                Present();
            }
            _previousTimestamp = timestamp;
        }

        private void OnDisplayDepthBufferChecked(object sender, RoutedEventArgs e)
        {
            _displayDepthBuffer = true;
        }

        private void OnDisplayDepthBufferUnchecked(object sender, RoutedEventArgs e)
        {
            _displayDepthBuffer = false;
        }

        private void OnRotateModelChecked(object sender, RoutedEventArgs e)
        {
            _isRotating = true;
        }

        private void OnRotateModelUnchecked(object sender, RoutedEventArgs e)
        {
            _isRotating = false;
        }

        private void OnRotationXChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _rotationSpeed.X = (float)e.NewValue;
            _rotationXLabel.Content = $"X ({_rotationSpeed.X:F2})";
        }

        private void OnRotationYChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _rotationSpeed.Y = (float)e.NewValue;
            _rotationYLabel.Content = $"Y ({_rotationSpeed.Y:F2})";
        }

        private void OnRotationZChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _rotationSpeed.Z = (float)e.NewValue;
            _rotationZLabel.Content = $"Z ({_rotationSpeed.Z:F2})";
        }

        private void OnResetClicked(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        private void OnUseHWIntrinsicsChecked(object sender, RoutedEventArgs e)
        {
            _useHWIntrinsics = true;
        }

        private void OnUseHWIntrinsicsUnchecked(object sender, RoutedEventArgs e)
        {
            _useHWIntrinsics = false;
        }

        private void OnWireframeChecked(object sender, RoutedEventArgs e)
        {
            _isWireframe = true;
        }

        private void OnWireframeUnchecked(object sender, RoutedEventArgs e)
        {
            _isWireframe = false;
        }

        private void OnZoomChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _scale.X = (float)e.NewValue;
            _scale.Y = (float)e.NewValue;
            _zoomLabel.Content = $"Zoom ({_scale.X:F2})";
        }

        private void SceneListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedIndex = Math.Clamp(_sceneListBox.SelectedIndex, 0, _scenes.Count - 1);

            if (_activeScene != null)
            {
                _activeScene.Clear();
            }

            _activeScene = _scenes[selectedIndex];
        }
        #endregion

        #region Methods
        private static Timestamp GetTimestamp()
        {
            double ticks = Stopwatch.GetTimestamp();
            ticks *= TickFrequency;
            return new Timestamp((long)ticks);
        }

        private void CameraToScreen(Model model)
        {
        }

        private void LoadFile(string path)
        {
            var item = new ListBoxItem();
            {
                item.Content = Path.GetFileNameWithoutExtension(path);
                item.IsEnabled = false;
            }
            _sceneListBox.Items.Add(item);

            var index = _scenes.Count;
            _scenes.Add(null);

            Task.Run(() => {
                _scenes[index] = Model.ParseJsonFile(path);
                Dispatcher.Invoke(() => item.IsEnabled = true);
            });
        }

        private void LoadScenes()
        {
            var item = new ListBoxItem();
            {
                item.Content = "None";
            }
            _sceneListBox.Items.Add(item);
            _scenes.Add(null);

            var directoryPath = Path.Combine(Environment.CurrentDirectory, "models");

            foreach (var modelFile in Directory.EnumerateFiles(directoryPath, "*.json"))
            {
                LoadFile(modelFile);
            }
        }

        private void ObjectToWorld(Model model)
        {
            RotateObject(model);
            ScaleObject(model);
            TranslateObject(model);
        }

        private void Present()
        {
            var displayBufferIndex = _renderBufferIndex++;

            if (_renderBufferIndex == BufferCount)
            {
                _renderBufferIndex = 0;
            }

            var displayBuffer = _buffers[displayBufferIndex];
            displayBuffer.Unlock();

            _displaySurface.Source = _displayDepthBuffer ? displayBuffer.DepthBuffer : displayBuffer.RenderBuffer;
            _fps++;
            _totalFrames++;
        }

        private void Render()
        {
            RenderInfo();
            RenderBuffer();
        }

        private void RenderBuffer()
        {
            var renderBuffer = _buffers[_renderBufferIndex];

            renderBuffer.Lock();
            renderBuffer.Clear(_backgroundColor, _clearDepth, _useHWIntrinsics);

            if (_activeScene != null)
            {
                renderBuffer.DrawModel(_activeScene, _foregroundColor, _isWireframe, _useHWIntrinsics);
            }
            renderBuffer.Invalidate();
        }

        private void RenderInfo()
        {
            if (_lastHeaderUpdate.Ticks < TimeSpan.TicksPerSecond)
            {
                return;
            }

            _minFps = Math.Min(_minFps, _fps);
            _maxFps = Math.Max(_maxFps, _fps);

            Title = $"FPS: {_fps}; Min FPS: {_minFps}; Max FPS: {_maxFps}; Avg FPS: {_totalFrames / (_totalUptime.Ticks / TicksPerSecond):F2}; Resolution: {(int)_displaySurface.Width}x{(int)_displaySurface.Height}; Vertices: {((_activeScene is null) ? 0 : _activeScene.ModifiedVertices.Count)}";

            _lastHeaderUpdate -= OneSecond;
            _fps = 0;
        }

        private void Reset()
        {
            _rotationXSlider.Value = DefaultRotationXSpeed;
            _rotationYSlider.Value = DefaultRotationYSpeed;
            _rotationZSlider.Value = DefaultRotationZSpeed;

            _zoomSlider.Value = DefaultZoomLevel;

            _displayDepthBufferCheckBox.IsChecked = false;
            _rotateModelCheckBox.IsChecked = false;
            _useHWIntrinsicsCheckBox.IsChecked = false;
            _wireframeCheckBox.IsChecked = true;

            _rotation = DefaultRotation;
            _scale = DefaultScale;
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

            var m = new Matrix3x3(
                new Vector3(scale.X, 0.0f, 0.0f),
                new Vector3(0.0f, scale.Y, 0.0f),
                new Vector3(0.0f, 0.0f, scale.Z)
            );

            for (var i = 0; i < polygon.Vertices.Count; i++)
            {
                polygon.ModifiedVertices[i] = polygon.ModifiedVertices[i].Transform(m);
            }
        }

        private void Startup()
        {
            LoadScenes();
            Reset();

            _rotateModelCheckBox.IsChecked = true;

            _camera.SetEyeAtUp(Vector3.Zero, Vector3.Zero, Vector3.UnitY);
            _camera.SetClip(1.0f, 10000.0f);

            _previousTimestamp = GetTimestamp();

            Dispatcher.Hooks.DispatcherInactive += OnApplicationIdle;
        }

        private void TranslateObject(Model polygon)
        {
            var translation = _translation;

            for (var i = 0; i < polygon.Vertices.Count; i++)
            {
                polygon.ModifiedVertices[i] += translation;
            }
        }

        private void Update(TimeSpan delta)
        {
            _totalUptime += delta;
            _lastHeaderUpdate += delta;

            UpdateBuffer();
            UpdateRotation(delta);

            var activeScene = _activeScene;

            if (activeScene != null)
            {
                activeScene.Reset();
                ObjectToWorld(activeScene);
                WorldToCamera(activeScene);
                CameraToScreen(activeScene);
            }
        }

        private void UpdateBuffer()
        {
            var displaySurface = _displaySurface;

            var pixelWidth = (int)displaySurface.Width;
            var pixelHeight = (int)displaySurface.Height;
            var pixelCount = pixelWidth * pixelHeight;

            var renderSurfaceIndex = _renderBufferIndex;
            var renderBuffer = _buffers[renderSurfaceIndex];

            if ((pixelCount != renderBuffer.PixelCount) || (pixelHeight != renderBuffer.PixelHeight) || (pixelWidth != renderBuffer.PixelWidth))
            {
                _translation = new Vector3(pixelWidth / 2.0f, pixelHeight / 2.0f, 0.0f);
                _buffers[renderSurfaceIndex] = new Bitmap(pixelWidth, pixelHeight);
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
            for (var i = 0; i < polygon.Vertices.Count; i++)
            {
                polygon.ModifiedVertices[i] = polygon.ModifiedVertices[i].Transform(_camera.ViewProjection);
            }

            for (var i = 0; i < polygon.Normals.Count; i++)
            {
                polygon.ModifiedNormals[i] = polygon.ModifiedNormals[i].Transform(_camera.ViewProjection);
            }
        }
        #endregion
    }
}
