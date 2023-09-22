using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SkiaSharp;

namespace TestSkia;

public class SkiaImage : FrameworkElement
{
    private readonly bool _designMode;
    private WriteableBitmap? _bitmap;
    private SKImage? _image;

    public SKImage? Source
    { 
        get
        {
            return _image;
        }
        set
        {
            lock(this)
            {
                _image = value;
            }
        }
    }

    public SkiaImage()
    {
        _designMode = DesignerProperties.GetIsInDesignMode(this);
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        if (_designMode)
            return;

        if (Visibility != Visibility.Visible || PresentationSource.FromVisual(this) == null)
            return;

        if (Source == null)
            return;

        lock (this)
        {
            SKImageInfo info = new(Source.Width, Source.Height);

            // Reset the bitmap if the size has changed.
            if (_bitmap == null || info.Width != _bitmap.PixelWidth || info.Height != _bitmap.PixelHeight)
            {
                _bitmap = new WriteableBitmap(info.Width, info.Height, 96, 96, PixelFormats.Pbgra32, null);
            }

            // Draw on the bitmap.
            _bitmap.Lock();
            using SKPixmap pixmap = new(info, _bitmap.BackBuffer, _bitmap.BackBufferStride);
            Source.ReadPixels(pixmap, 0, 0);

            _bitmap.AddDirtyRect(new Int32Rect(0, 0, info.Width, info.Height));
        }

        _bitmap.Unlock();
        drawingContext.DrawImage(_bitmap, new Rect(0, 0, ActualWidth, ActualHeight));
    }
}