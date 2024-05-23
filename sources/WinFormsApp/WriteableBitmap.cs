// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace WinFormsApp;

public sealed class WriteableBitmap(int pixelWidth, int pixelHeight, PixelFormat pixelFormat)
{
    private readonly Bitmap _bitmap = new Bitmap(pixelWidth, pixelHeight, pixelFormat);
    private BitmapData? _bitmapData;

    public void Lock(Rectangle dirtyRegion) => _bitmapData = _bitmap.LockBits(dirtyRegion, ImageLockMode.ReadWrite, _bitmap.PixelFormat);

    public void Unlock()
    {
        Debug.Assert(_bitmapData is not null);
        _bitmap.UnlockBits(_bitmapData);
        _bitmapData = null;
    }

    public IntPtr BackBuffer
    {
        get
        {
            Debug.Assert(_bitmapData is not null);
            return _bitmapData.Scan0;
        }
    }

    public int PixelWidth => _bitmap.Width;

    public int PixelHeight => _bitmap.Height;

    public static implicit operator Image(WriteableBitmap value) => value._bitmap;
}
