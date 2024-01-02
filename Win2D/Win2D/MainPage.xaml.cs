using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Win2D
{
	public sealed partial class MainPage : Page
	{
		private readonly Random _random = new Random();
		private readonly FrameRateMonitor _frameRateMonitor = new FrameRateMonitor();

		private const int NumberOfLines = 5_000;

		public MainPage()
		{
			this.InitializeComponent();

			CompositionTarget.Rendering += CompositionTarget_Rendering;

			_frameRateMonitor.Start();
			
		}

		private void CompositionTarget_Rendering(object sender, object e)
		{
			MyCanvas.Invalidate();
		}

		public void CanvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
		{
			var session = args.DrawingSession;

			for (int i = 0; i < 5;/* NumberOfLines;*/ i++)
			{
				var width = (int)sender.ActualWidth;
				var height = (int)sender.ActualHeight;

				var point1 = RandomPoint(width, height);
				var point2 = RandomPoint(width, height);
				var color = RandomColor();
				var strokeWidth = _random.Next(1, 10);

				session.DrawLine(point1, point2, color, strokeWidth);
			}

			_frameRateMonitor.DrawCalled();

			var frameRate = _frameRateMonitor.FrameRate;
			var drawRate = _frameRateMonitor.DrawRate;

			session.DrawText($"Frame Rate: {frameRate:0.0} - Draw Rate: {drawRate:0.0}", new Vector2(0, 0), Colors.Black);
		}

		private Vector2 RandomPoint(int width, int height)
		{
			return new Vector2(_random.Next(width), _random.Next(height));
		}

		private Color RandomColor()
		{
			return Color.FromArgb(255, (byte)_random.Next(255), (byte)_random.Next(255), (byte)_random.Next(255));
		}
	}
}