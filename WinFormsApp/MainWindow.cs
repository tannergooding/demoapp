// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using BitmapRendering;

namespace WinFormsApp
{
    public partial class MainWindow : Form
    {
        private const int BufferCount = 2;

        private static readonly PixelFormat DepthBufferPixelFormat = PixelFormat.Format32bppArgb;
        private static readonly PixelFormat RenderBufferPixelFormat = PixelFormat.Format32bppArgb;

        private readonly BitmapRenderer _renderer = new BitmapRenderer();
        private readonly List<Model?> _scenes = new List<Model?>();
        private readonly (WriteableBitmap Render, WriteableBitmap Depth)[] _buffers = new (WriteableBitmap, WriteableBitmap)[BufferCount];

        private int _bufferIndex = 0;

        public MainWindow()
        {
            InitializeComponent();
            Startup();
        }

        private void OnApplicationIdle(object? sender, EventArgs e)
        {
            var buffer = GetBuffer(_bufferIndex);

            var dirtyRegion = new Rectangle(0, 0, buffer.Render.PixelWidth, buffer.Render.PixelHeight);

            buffer.Render.Lock(dirtyRegion);
            buffer.Depth.Lock(dirtyRegion);
            {
                _renderer.Update(buffer.Render.BackBuffer, buffer.Depth.BackBuffer, buffer.Render.PixelWidth, buffer.Render.PixelHeight);
                _renderer.Render();
                _renderer.Present();

                var nextBufferIndex = _bufferIndex++;

                if (_bufferIndex == BufferCount)
                {
                    _bufferIndex = 0;
                }

                var nextBuffer = GetBuffer(nextBufferIndex);

                nextBuffer.Render.Unlock();
                nextBuffer.Depth.Unlock();
            }

            if (_renderer.Title != Text)
            {
                Text = _renderer.Title;
            }
            _displaySurface.Image = _renderer.DisplayDepthBuffer ? buffer.Depth : buffer.Render;
        }

        private void OnDisplayDepthBufferCheckedChanged(object sender, EventArgs e)
        {
            _renderer.DisplayDepthBuffer = _displayDepthBufferCheckBox.Checked;
        }

        private void OnLightPositionXChanged(object sender, EventArgs e)
        {
            _renderer.LightPositionX = _lightPositionXSlider.Value;
            _lightPositionXLabel.Text = $"X ({_renderer.LightPositionX:F2})";
        }

        private void OnLightPositionYChanged(object sender, EventArgs e)
        {
            _renderer.LightPositionY = _lightPositionYSlider.Value;
            _lightPositionYLabel.Text = $"Y ({_renderer.LightPositionY:F2})";
        }

        private void OnLightPositionZChanged(object sender, EventArgs e)
        {
            _renderer.LightPositionZ = _lightPositionZSlider.Value;
            _lightPositionZLabel.Text = $"Z ({_renderer.LightPositionZ:F2})";
        }
        private void OnResetClicked(object sender, EventArgs e) => Reset();

        private void OnRotateModelCheckedChanged(object sender, EventArgs e)
        {
            _renderer.RotateModel = _rotateModelCheckBox.Checked;
        }

        private void OnRotationXChanged(object sender, EventArgs e)
        {
            _renderer.RotationXSpeed = _rotationXSlider.Value;
            _rotationXLabel.Text = $"X ({_renderer.RotationXSpeed:F2})";
        }

        private void OnRotationYChanged(object sender, EventArgs e)
        {
            _renderer.RotationYSpeed = _rotationYSlider.Value;
            _rotationYLabel.Text = $"Y ({_renderer.RotationYSpeed:F2})";
        }

        private void OnRotationZChanged(object sender, EventArgs e)
        {
            _renderer.RotationZSpeed = _rotationZSlider.Value;
            _rotationZLabel.Text = $"Z ({_renderer.RotationZSpeed:F2})";
        }

        private void OnUseHWIntrinsicsCheckedChanged(object sender, EventArgs e)
        {
            _renderer.UseHWIntrinsics = _useHWIntrinsicsCheckBox.Checked;
        }

        private void OnWireframeCheckedChanged(object sender, EventArgs e)
        {
            _renderer.Wireframe = _wireframeCheckBox.Checked;
        }

        private void OnZoomChanged(object sender, EventArgs e)
        {
            _renderer.ZoomLevel = _zoomSlider.Value;
            _zoomLabel.Text = $"Zoom ({_renderer.ZoomLevel:F2})";
        }

        private void SceneListBox_SelectionChanged(object sender, EventArgs e)
        {
            var selectedIndex = Math.Clamp(_sceneListBox.SelectedIndex, 0, _scenes.Count - 1);

            if (_renderer.ActiveScene != null)
            {
                _renderer.ActiveScene.Clear();
            }

            _renderer.ActiveScene = _scenes[selectedIndex];
        }

        private void LoadFile(string path)
        {
            _sceneListBox.Items.Add(Path.GetFileNameWithoutExtension(path));
            _scenes.Add(Model.ParseJsonFile(path));
        }

        private void LoadScenes()
        {
            _sceneListBox.Items.Add("None");
            _scenes.Add(null);

            var directoryPath = Path.Combine(Environment.CurrentDirectory, "models");

            foreach (var modelFile in Directory.EnumerateFiles(directoryPath, "*.json"))
            {
                LoadFile(modelFile);
            }
        }

        private void Reset()
        {
            _renderer.Reset();

            _displayDepthBufferCheckBox.Checked = _renderer.DisplayDepthBuffer;
            _lightPositionXSlider.Value = (int)_renderer.LightPositionX;
            _lightPositionYSlider.Value = (int)_renderer.LightPositionY;
            _lightPositionZSlider.Value = (int)_renderer.LightPositionZ;
            _rotateModelCheckBox.Checked = _renderer.RotateModel;
            _rotationXSlider.Value = (int)_renderer.RotationXSpeed;
            _rotationYSlider.Value = (int)_renderer.RotationYSpeed;
            _rotationZSlider.Value = (int)_renderer.RotationZSpeed;
            _useHWIntrinsicsCheckBox.Checked = _renderer.UseHWIntrinsics;
            _wireframeCheckBox.Checked = _renderer.Wireframe;
            _zoomSlider.Value = (int)_renderer.ZoomLevel;
        }

        private void Startup()
        {
            Reset();
            LoadScenes();
            Application.Idle += OnApplicationIdle;
        }

        private (WriteableBitmap Render, WriteableBitmap Depth) GetBuffer(int index)
        {
            var displaySurface = _displaySurface;

            var pixelWidth = (int)displaySurface.Width;
            var pixelHeight = (int)displaySurface.Height;

            var buffer = _buffers[index];

            if ((buffer.Render is null) || (pixelWidth != buffer.Render.PixelWidth) || (pixelHeight != buffer.Render.PixelHeight))
            {
                buffer.Render = new WriteableBitmap(pixelWidth, pixelHeight, RenderBufferPixelFormat);
                buffer.Depth = new WriteableBitmap(pixelWidth, pixelHeight, DepthBufferPixelFormat);
                _buffers[index] = buffer;
            }
            return buffer;
        }
    }
}
