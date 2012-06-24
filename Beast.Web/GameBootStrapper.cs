using System;
using System.Configuration;
using System.Web.Hosting;
using Beast.Configuration;

namespace Beast.Web
{
	public static class GameBootStrapper
	{
		public static bool Initialized { get; private set; }
		private static readonly object LockObject = new object();
		private static readonly ShutdownDetector Detector = new ShutdownDetector(OnAppDomainShutdown);

		public static void Initialize()
		{
			if (!Initialized)
			{
				lock (LockObject)
				{
					if (!Initialized)
					{
						if (Game.Current.IsRunning)
							return;

						// Resolve the config section
						var section = BeastSection.Load();
						if (section == null)
						{
							throw new ConfigurationErrorsException(CommonResources.ConfigBeastSectionNotFound);
						}

						// Start the Game
						Game.Current.Start(section);

						Initialized = true;
					}
				}
			}
		}

		private static void OnAppDomainShutdown()
		{
			Game.Current.Stop();
		}

		private class ShutdownDetector : IRegisteredObject
		{
			private readonly Action _onShutdown;

			public ShutdownDetector(Action onShutdown)
			{
				_onShutdown = onShutdown;
				HostingEnvironment.RegisterObject(this);
			}

			public void Stop(bool immediate)
			{
				try
				{
					if (!immediate)
					{
						_onShutdown();
					}
				}
				catch (Exception ex)
				{
					Log.Error(ex);
				}
				finally
				{
					HostingEnvironment.UnregisterObject(this);
				}
			}
		}
	}
}
