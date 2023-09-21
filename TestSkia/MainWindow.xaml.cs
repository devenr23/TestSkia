using System;
using System.Windows;
using System.Windows.Media;
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

    private SKColor _background1 = SKColors.CornflowerBlue;
    private SKColor _background2 = SKColors.Orchid;
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowVM();

        CompositionTarget.Rendering += CompositionTarget_Rendering;
    }

    private void CompositionTarget_Rendering(object? sender, EventArgs e)
    {
        if (Animate.IsChecked ?? false)
        {
            if (ViewSkia.IsChecked ?? false)
            {
                SkiaElement.InvalidateVisual();
            }
            else
            {
                GlElement.InvalidateVisual();
            }
        }
    }

    private void SkiaElement_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
    {
        DrawCanvas(e.Surface.Canvas, e.Info.Width, e.Info.Height, _background1);
    }

    private void DrawCanvas(SKCanvas canvas, int width, int height, SKColor background)
    {
        canvas.Clear(background);

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

    private void SKGLelement_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
    {
        DrawCanvas(e.Surface.Canvas, e.Info.Width, e.Info.Height,_background2);
    }
}