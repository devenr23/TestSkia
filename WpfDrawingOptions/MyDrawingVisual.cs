﻿using System;
using System.Windows;
using System.Windows.Media;

namespace WpfDrawingOptions;
public class MyDrawingVisual : FrameworkElement
{
    // Create a collection of child visual objects.
    private readonly VisualCollection _children;
    private readonly DrawingVisual _visual = new();
    private readonly Random _random = new();

    public MyDrawingVisual()
    {
        _children = new VisualCollection(this) { _visual };
    }

    public void Draw()
    {
        // Retrieve the DrawingContext in order to create new drawing content.
        var context = _visual.RenderOpen();

        for (var i = 0; i < TestConstants.NumberOfLines; i++)
        {
            var color = new Color
            {
                R = (byte)_random.Next(255),
                G = (byte)_random.Next(255),
                B = (byte)_random.Next(255),
                A = (byte)_random.Next(255)
            };

            var pen = new Pen
            {
                Brush = new SolidColorBrush(color),
                Thickness = _random.Next(1, 10),
            };
            context.DrawLine(pen, 
                new Point(_random.Next((int) ActualWidth), _random.Next((int) ActualHeight)), 
                new Point(_random.Next((int) ActualWidth), _random.Next((int) ActualHeight)));
        }

        // Persist the drawing content.
        context.Close();

        FrameRateMonitor.Instance.DrawCalled();
    }

    // Provide a required override for the VisualChildrenCount property.
    protected override int VisualChildrenCount => 1;

    // Provide a required override for the GetVisualChild method.
    protected override Visual GetVisualChild(int index)
    {
        if (index != 0)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        return _visual;
    }
}