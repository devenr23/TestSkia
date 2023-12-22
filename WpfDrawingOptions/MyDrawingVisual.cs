using System;
using System.Windows;
using System.Windows.Media;

namespace WpfDrawingOptions;
public class MyDrawingVisual : FrameworkElement
{
    // Create a collection of child visual objects.
    private readonly VisualCollection _children;
    private readonly Random _random = new();
    private const int NUM_LINES = 5000;

    public MyDrawingVisual()
    {
        _children = new VisualCollection(this);
        for (var i = 0; i < NUM_LINES; i++)
        {
            _children.Add(new DrawingVisual());
        }
    }

    public void Draw()
    {
        for (int i = 0; i < NUM_LINES; i++)
        {
            var color = new Color
            {
                R = (byte)_random.Next(255),
                G = (byte)_random.Next(255),
                B = (byte)_random.Next(255),
                A = (byte)_random.Next(255)
            };

            DrawLine(
                i,
                _random.Next((int) ActualWidth),
                _random.Next((int) ActualHeight),
                _random.Next((int) ActualWidth),
                _random.Next((int) ActualHeight),
                color,
                _random.Next(1, 10));
        }
    }

    private void DrawLine(int childIndex, int x1, int y1, int x2, int y2, Color color, int strokeWidth)
    {
        var drawingVisual = (DrawingVisual) _children[childIndex];

        // Retrieve the DrawingContext in order to create new drawing content.
        DrawingContext drawingContext = drawingVisual.RenderOpen();

        var pen = new Pen
        {
            Brush = new SolidColorBrush(color),
            Thickness = strokeWidth,
        };
        drawingContext.DrawLine(pen, new Point(x1, y1), new Point(x2, y2));

        // Persist the drawing content.
        drawingContext.Close();
    }

    // Provide a required override for the VisualChildrenCount property.
    protected override int VisualChildrenCount => _children.Count;

    // Provide a required override for the GetVisualChild method.
    protected override Visual GetVisualChild(int index)
    {
        if (index < 0 || index >= _children.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        return _children[index];
    }
}