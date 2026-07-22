// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using BitmapRendering;

namespace WpfApp;

internal partial class MainWindow : Window
{
    private const int BufferCount = 2;

    private static readonly PixelFormat s_depthBufferPixelFormat = PixelFormats.Gray32Float;
    private static readonly PixelFormat s_renderBufferPixelFormat = PixelFormats.Bgra32;

    private readonly BitmapRenderer _renderer = new BitmapRenderer();
    private readonly List<Model?> _scenes = [];
    private readonly (WriteableBitmap Render, WriteableBitmap Depth)[] _buffers = new (WriteableBitmap, WriteableBitmap)[BufferCount];

    private int _bufferIndex;

    public MainWindow()
    {
        InitializeComponent();
        Startup();
    }

    private void OnApplicationIdle(object? sender, EventArgs e)
    {
        var (render, depth) = GetBuffer(_bufferIndex);

        render.Lock();
        depth.Lock();
        {
            _renderer.Update(render.BackBuffer, depth.BackBuffer, render.PixelWidth, render.PixelHeight);
            _renderer.Render();
            _renderer.Present();

            // Gray32Float shows the raw depth, and reverse-Z leaves the geometry in a sliver
            // near zero, so normalize it for contrast before display.
            if (_renderer.DisplayDepthBuffer)
            {
                _renderer.VisualizeDepth(depth.BackBuffer, render.PixelWidth * render.PixelHeight);
            }

            var dirtyRegion = new Int32Rect(0, 0, render.PixelWidth, render.PixelHeight);
            render.AddDirtyRect(dirtyRegion);
            depth.AddDirtyRect(dirtyRegion);

            var currentBufferIndex = _bufferIndex;
            _bufferIndex = (_bufferIndex + 1) % BufferCount;

            var (currentRender, currentDepth) = GetBuffer(currentBufferIndex);

            currentRender.Unlock();
            currentDepth.Unlock();
        }

        if (_renderer.Title != Title)
        {
            Title = _renderer.Title;
        }
        _displaySurface.Source = _renderer.DisplayDepthBuffer ? depth : render;
    }

    private void OnDisplayDepthBufferChecked(object sender, RoutedEventArgs e) => _renderer.DisplayDepthBuffer = true;

    private void OnDisplayDepthBufferUnchecked(object sender, RoutedEventArgs e) => _renderer.DisplayDepthBuffer = false;

    private void OnLightPositionXChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        _renderer.LightPositionX = (float)e.NewValue;
        _lightPositionXLabel.Content = $"X ({_renderer.LightPositionX:F2})";
    }

    private void OnLightPositionYChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        _renderer.LightPositionY = (float)e.NewValue;
        _lightPositionYLabel.Content = $"Y ({_renderer.LightPositionY:F2})";
    }

    private void OnLightPositionZChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        _renderer.LightPositionZ = (float)e.NewValue;
        _lightPositionZLabel.Content = $"Z ({_renderer.LightPositionZ:F2})";
    }

    private void OnResetClicked(object sender, RoutedEventArgs e) => Reset();

    private void OnRotateModelChecked(object sender, RoutedEventArgs e) => _renderer.RotateModel = true;

    private void OnRotateModelUnchecked(object sender, RoutedEventArgs e) => _renderer.RotateModel = false;

    private void OnRotationXChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        _renderer.RotationXSpeed = (float)e.NewValue;
        _rotationXLabel.Content = $"X ({_renderer.RotationXSpeed:F2})";
    }

    private void OnRotationYChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        _renderer.RotationYSpeed = (float)e.NewValue;
        _rotationYLabel.Content = $"Y ({_renderer.RotationYSpeed:F2})";
    }

    private void OnRotationZChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        _renderer.RotationZSpeed = (float)e.NewValue;
        _rotationZLabel.Content = $"Z ({_renderer.RotationZSpeed:F2})";
    }

    private void OnUseHWIntrinsicsChecked(object sender, RoutedEventArgs e) => _renderer.UseHWIntrinsics = true;

    private void OnUseHWIntrinsicsUnchecked(object sender, RoutedEventArgs e) => _renderer.UseHWIntrinsics = false;

    private void OnWireframeChecked(object sender, RoutedEventArgs e) => _renderer.Wireframe = true;

    private void OnWireframeUnchecked(object sender, RoutedEventArgs e) => _renderer.Wireframe = false;

    private void OnZoomChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        _renderer.ZoomLevel = (float)e.NewValue;
        _zoomLabel.Content = $"Zoom ({_renderer.ZoomLevel:F2})";
    }

    private void SceneListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedIndex = int.Clamp(_sceneListBox.SelectedIndex, 0, _scenes.Count - 1);

        _renderer.ActiveScene?.Clear();

        _renderer.ActiveScene = _scenes[selectedIndex];
    }

    private void LoadFile(string path)
    {
        var item = new ListBoxItem();
        {
            item.Content = Path.GetFileNameWithoutExtension(path);
            item.IsEnabled = false;
        }
        _ = _sceneListBox.Items.Add(item);

        var index = _scenes.Count;
        _scenes.Add(null);

        _ = Task.Run(() => {
            _scenes[index] = Model.ParseJsonFile(path);
            _ = Dispatcher.Invoke(() => item.IsEnabled = true);
        });
    }

    private void LoadScenes()
    {
        var item = new ListBoxItem();
        {
            item.Content = "None";
        }
        _ = _sceneListBox.Items.Add(item);
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

        _displayDepthBufferCheckBox.IsChecked = _renderer.DisplayDepthBuffer;
        _lightPositionXSlider.Value = _renderer.LightPositionX;
        _lightPositionYSlider.Value = _renderer.LightPositionY;
        _lightPositionZSlider.Value = _renderer.LightPositionZ;
        _rotateModelCheckBox.IsChecked = _renderer.RotateModel;
        _rotationXSlider.Value = _renderer.RotationXSpeed;
        _rotationYSlider.Value = _renderer.RotationYSpeed;
        _rotationZSlider.Value = _renderer.RotationZSpeed;
        _useHWIntrinsicsCheckBox.IsChecked = _renderer.UseHWIntrinsics;
        _wireframeCheckBox.IsChecked = _renderer.Wireframe;
        _zoomSlider.Value = _renderer.ZoomLevel;
    }

    private void Startup()
    {
        Reset();
        LoadScenes();
        Dispatcher.Hooks.DispatcherInactive += OnApplicationIdle;
    }

    private (WriteableBitmap Render, WriteableBitmap Depth) GetBuffer(int index)
    {
        var displaySurface = _displaySurface;
        var dpi = VisualTreeHelper.GetDpi(displaySurface);

        // Size the backbuffers in physical pixels so the software rasterizer renders at the
        // display's real resolution. Sizing them in DIPs would let WPF upscale and blur the
        // result on a high-DPI monitor.
        var pixelWidth = int.Max(1, (int)(displaySurface.ActualWidth * dpi.DpiScaleX));
        var pixelHeight = int.Max(1, (int)(displaySurface.ActualHeight * dpi.DpiScaleY));

        var buffer = _buffers[index];

        if ((buffer.Render is null) || (pixelWidth != buffer.Render.PixelWidth) || (pixelHeight != buffer.Render.PixelHeight))
        {
            buffer.Render = new WriteableBitmap(pixelWidth, pixelHeight, 96.0 * dpi.DpiScaleX, 96.0 * dpi.DpiScaleY, s_renderBufferPixelFormat, palette: null);
            buffer.Depth = new WriteableBitmap(pixelWidth, pixelHeight, 96.0 * dpi.DpiScaleX, 96.0 * dpi.DpiScaleY, s_depthBufferPixelFormat, palette: null);
            _buffers[index] = buffer;
        }
        return buffer;
    }
}
