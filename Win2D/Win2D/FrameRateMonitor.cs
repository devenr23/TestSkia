using System.Diagnostics;
using Windows.UI.Xaml.Media;

public class FrameRateMonitor
{
	private readonly Stopwatch _renderMonitorStopwatch;
	private int _renderCount = 0;

	private readonly Stopwatch _drawMonitorStopwatch;
	private int _drawCount = 0;

	public FrameRateMonitor()
	{
		_renderMonitorStopwatch = new Stopwatch();
		_drawMonitorStopwatch = new Stopwatch();

		Start();
	}

	public void Start()
	{
		_renderMonitorStopwatch.Start();
		_renderCount = 0;

		_drawMonitorStopwatch.Start();
		_drawCount = 0;

		CompositionTarget.Rendering += CompositionTargetOnRendering;
	}

	/// <summary>
	/// This is called every time WPF is about to render a frame
	/// </summary>
	private void CompositionTargetOnRendering(object sender, object e)
	{
		lock (_renderMonitorStopwatch)
		{
			_renderCount++;

			var elapsedSeconds = _renderMonitorStopwatch.ElapsedMilliseconds / 1000.0;
			if (elapsedSeconds > 0.05)
			{
				FrameRate = _renderCount / elapsedSeconds;
			}

			if (elapsedSeconds > 1)
			{
				_renderCount = 0;
				_renderMonitorStopwatch.Restart();
			}
		}
	}
	
	public void DrawCalled()
	{
		lock (_drawMonitorStopwatch)
		{
			_drawCount++;

			var elapsedSeconds = _drawMonitorStopwatch.ElapsedMilliseconds / 1000.0;
			if (elapsedSeconds > 0.05)
			{
				DrawRate = _drawCount / elapsedSeconds;
			}

			if (elapsedSeconds > 1)
			{
				_drawCount = 0;
				_drawMonitorStopwatch.Restart();
			}
		}
	}

	public double FrameRate { get; private set; }
	public double DrawRate { get; private set; }
	
	public void Stop()
	{
		CompositionTarget.Rendering -= CompositionTargetOnRendering;
		
		_renderMonitorStopwatch.Stop();
		_renderCount = 0;
		
		_drawMonitorStopwatch.Stop();
		_drawCount = 0;
	}
}