using System;
using System.Windows;
using System.Windows.Forms.Integration;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using TestSkia.ViewModels;

namespace TestSkia;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly SKPaint _paint = new SKPaint
    {
        IsAntialias = true,
        Style = SKPaintStyle.Stroke
    };
    private readonly Random _random = new Random();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowVM();
    }

    private void SkiaElement_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
    {
        DrawCanvas(e.Surface.Canvas, e.Info.Width, e.Info.Height);
    }

    private void GlElement_Initialized(object sender, System.EventArgs e)
    {
        var glControl = new SKGLControl();
        glControl.PaintSurface += (sender, e) => DrawCanvas(e.Surface.Canvas, e.BackendRenderTarget.Width, e.BackendRenderTarget.Height);
        glControl.Dock = System.Windows.Forms.DockStyle.Fill;

        var host = (WindowsFormsHost)sender;
        host.Child = glControl;
    }

    private void DrawCanvas(SKCanvas canvas, int width, int height)
    {
        canvas.Clear(SKColor.Parse("#003366"));

        for (int i = 0; i < 1000; i++)
        {
            _paint.Color = new SKColor(
                red: (byte)_random.Next(255),
                green: (byte)_random.Next(255),
                blue: (byte)_random.Next(255),
                alpha: (byte)_random.Next(255));

            _paint.StrokeWidth = _random.Next(1, 10);

            canvas.DrawLine(
                x0: _random.Next(width),
                y0: _random.Next(height),
                x1: _random.Next(width),
                y1: _random.Next(height),
                paint: _paint);
        }
    }
}