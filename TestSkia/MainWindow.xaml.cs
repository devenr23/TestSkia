﻿using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace TestSkia;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly SKColor _background1 = SKColors.MediumVioletRed;
    private readonly SKColor _background2 = SKColors.GreenYellow;
    private readonly SKColor _background3 = SKColors.DeepSkyBlue;
    private readonly SKPaint _paint = new()
    {
        IsAntialias = true,
        Style = SKPaintStyle.Stroke
    };
    private readonly Random _random = new();
    private bool _cancelImageTask;

    public MainWindow()
    {
        InitializeComponent();

        CompositionTarget.Rendering += CompositionTarget_Rendering;
    }

    private void CompositionTarget_Rendering(object? sender, EventArgs e)
    {
        if (UseSkElement.IsChecked ?? false)
        {
            SkiaElement.InvalidateVisual();
        }
        else if (UseSkGl.IsChecked ?? false)
        {
            SkGlElement.InvalidateVisual();
        }
        else if (UseDrawingVisual.IsChecked ?? false)
        {
            DrawingVisualElement.Draw();
        }
        else if (UseDrawingCanvas.IsChecked ?? false)
        {
            DrawingCanvasElement.InvalidateVisual();
        }
    }

    private void DrawCanvas(SKCanvas canvas, int width, int height, SKColor background)
    {
        canvas.Clear(background);

        for (int i = 0; i < 5000; i++)
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

    private void DrawTask()
    {
        const double maxFps = 30;
        double minFramePeriodMsec = 1000.0 / maxFps;

        Stopwatch stopwatch = Stopwatch.StartNew();
        while (!_cancelImageTask)
        {
            int width = 0;
            int height = 0;

            SkiaImageElement.Dispatcher.Invoke(() =>
            {
                width = (int)SkiaImageElement.ActualWidth;
                height = (int)SkiaImageElement.ActualHeight;
            });

            var bmp = new SKBitmap(width, height);
            using var canvas = new SKCanvas(bmp);

            DrawCanvas(canvas, width, height, _background3);

            SkiaImageElement.Source = SKImage.FromPixels(bmp.PeekPixels());

            SkiaImageElement.Dispatcher.BeginInvoke(() => SkiaImageElement.InvalidateVisual());

            // FPS limiter
            double msToWait = minFramePeriodMsec - stopwatch.ElapsedMilliseconds;
            if (msToWait > 0)
                Thread.Sleep((int)msToWait);
            stopwatch.Restart();
        }
    }

    private void SkGlElement_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
    {
        DrawCanvas(e.Surface.Canvas, e.Info.Width, e.Info.Height, _background2);
    }

    private void SkiaElement_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        DrawCanvas(e.Surface.Canvas, e.Info.Width, e.Info.Height, _background1);
    }

    private void UseSkiaImage_Checked(object sender, RoutedEventArgs e)
    {
        _cancelImageTask = false;
        Task.Run(DrawTask);
    }

    private void UseSkiaImage_Unchecked(object sender, RoutedEventArgs e)
    {
        _cancelImageTask = true;
    }
}