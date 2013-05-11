using System;
using System.Diagnostics;

namespace Beast
{
    /// <summary>
    /// Provides an object to track elapsed application time.
    /// </summary>
	public sealed class ApplicationTime : IDisposable
	{
		private readonly Stopwatch _clock = new Stopwatch();

        /// <summary>
        /// Gets the total elapsed time since the object was created.
        /// </summary>
		public TimeSpan Total { get; private set; }

        /// <summary>
        /// Gets the time that has elapsed since the last call to Update.
        /// </summary>
		public TimeSpan Elapsed { get; private set; }

        /// <summary>
        /// Gets the number of ticks that have elapsed since the object was created.
        /// </summary>
		public long Ticks { get; private set; }

        private TimeSpan _lastElapsed;

        /// <summary>
        /// Updates the elapsed time and ticks.
        /// </summary>
		public void Update()
		{
			if (!_clock.IsRunning)
				_clock.Start();

			Ticks += _clock.ElapsedTicks;
            Total += Elapsed;

            var elapsed = _clock.Elapsed;
            Elapsed = elapsed - _lastElapsed;
            _lastElapsed = elapsed;
		}

        /// <summary>
        /// Disposes of the object.
        /// </summary>
		public void Dispose()
		{
			if (_clock != null)
				_clock.Stop();
		}
	}
}