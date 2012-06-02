using System;
using System.Timers;

namespace Beast
{
	public class GameClock
	{
		public TimeSpan TotalGameTime { get; private set; }

		private DateTime _lastStep = DateTime.UtcNow;
		private Timer _timer;
		private Action _elapsedCallback;

		public void Start(double interval, Action elapsedCallback)
		{
			_elapsedCallback = elapsedCallback;
			_timer = new Timer(interval);
			_timer.Elapsed += OnTimerElapsed;
			_timer.Start();
		}

		public void Stop()
		{
			if (_timer != null)
			{
				_timer.Elapsed -= OnTimerElapsed;
				_timer.Dispose();
				_timer = null;
			}
		}

		private void OnTimerElapsed(object sender, ElapsedEventArgs e)
		{
			var now = DateTime.UtcNow;
			var elapsed = now - _lastStep;
			_lastStep = now;
			TotalGameTime += elapsed;
			//Log.Debug("Performing game clock step at {0}", TotalGameTime);
			if (_elapsedCallback != null)
				_elapsedCallback();
		}
	}
}