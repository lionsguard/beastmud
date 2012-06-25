using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Linq;
using Beast.Commands;
using Beast.Configuration;
using Beast.Data;
using Beast.IO;
using Beast.Net;
using Beast.Security;

namespace Beast
{
	/// <summary>
	/// Represents the game entry point and main loop.
	/// </summary>
	public class Game
	{
		/// <summary>
		/// Gets the default game step interval.
		/// </summary>
		public static readonly TimeSpan DefaultGameStepInterval = TimeSpan.FromSeconds(5);

		/// <summary>
		/// Gets the default connection timeout.
		/// </summary>
		public static readonly TimeSpan DefaultConnectionTimeout = TimeSpan.FromMinutes(15);

		#region Current
		private static readonly object InitLock = new object();
		private static Game _current;
		/// <summary>
		/// Gets the current Game instance.
		/// </summary>
		public static Game Current
		{
			get
			{
				if (_current == null)
				{
					lock (InitLock)
					{
						if (_current == null)
							_current = new Game();
					}
				}
				return _current;
			}
		}
		#endregion

		/// <summary>
		/// Gets or sets the current data repository.
		/// </summary>
		[Import(typeof(IRepository), AllowDefault = true)]
		public IRepository Repository { get; set; }

		[ImportMany(AllowRecomposition = true, RequiredCreationPolicy = CreationPolicy.Shared)]
		private IEnumerable<Lazy<IModule, IModuleMetaData>> LoadedModules { get; set; }

		[ImportMany(typeof(ILogger), AllowRecomposition = true)]
		private IEnumerable<ILogger> Loggers { get; set; }

		/// <summary>
		/// Gets a value indicating whether or not the game is currently running.
		/// </summary>
		public bool IsRunning { get; private set; }

		/// <summary>
		/// Gets the current GameTime.
		/// </summary>
		public GameTime GameTime { get; private set; }

		/// <summary>
		/// Gets the currently loaded world.
		/// </summary>
		public World World { get; private set; }

		/// <summary>
		/// Gets the currently loaded configuration section.
		/// </summary>
		public BeastSection Config { get; private set; }

		/// <summary>
		/// Gets the current CommandManager instance.
		/// </summary>
		public CommandManager Commands { get; private set; }

		private GameClock _clock;

		private List<IModule> _modules = new List<IModule>();

		private Game()
		{
			Commands = new CommandManager();
		}

		#region Start and Stop
		/// <summary>
		/// Starts the game and begins the main loop using the default game settings.
		/// </summary>
		public void Start()
		{
			Start(new BeastSection{Repository = new RepositoryElement{Type = typeof(FileRepository).AssemblyQualifiedName}});
		}

		/// <summary>
		/// Starts the game and begins the main loop using the specified configFilePath to load the game settings.
		/// </summary>
		/// <param name="configFilePath">The path to the configuration file used to load game settings.</param>
		public void Start(string configFilePath)
		{
			Start(BeastSection.Load(configFilePath));
		}

		/// <summary>
		/// Starts the game and begins the main loop.
		/// </summary>
		/// <param name="config">The configuration section used to initialize the game settings.</param>
		public void Start(BeastSection config)
		{
			if (IsRunning)
				return;

			// ====================================================================================
			// LOAD CONFIGURATION
			// ====================================================================================
			if (config == null)
			{
				throw new ConfigurationErrorsException();
			}
			Config = config;

			// ====================================================================================
			// INITIALIZE STARTUP VARIABLES
			// ====================================================================================
			// Signal that the game is running.
			IsRunning = true;
			_clock = new GameClock();
			GameTime = new GameTime();

			try
			{
				// ====================================================================================
				// LOAD MODULES FROM EXTERNAL SOURCES
				// ====================================================================================
				var catalog = new AggregateCatalog();
				catalog.Catalogs.Add(new AssemblyCatalog(typeof(Game).Assembly));

				if (config.ModulesDirectory != null)
				{
					var modulesDirectory = config.ModulesDirectory.Path;
					if (config.ModulesDirectory.IsVirtual)
					{
						// Using a virtual path
						throw new NotImplementedException();
					}

					if (!string.IsNullOrEmpty(modulesDirectory))
						catalog.Catalogs.Add(new DirectoryCatalog(modulesDirectory));
				}

				if (config.Modules != null && config.Modules.Count > 0)
				{
					var moduleTypes = config.Modules.Cast<ModuleElement>()
						.Select(moduleElement => Type.GetType(moduleElement.Type, false, true))
						.Where(moduleType => moduleType != null);

					catalog.Catalogs.Add(new TypeCatalog(moduleTypes));
				}

				var container = new CompositionContainer(catalog);
				container.ComposeParts(this, Commands);

				// ====================================================================================
				// INITIALIZE LOGGING
				// ====================================================================================
				Log.Initialize(Loggers);

				// ====================================================================================
				// INITIALIZE BASIC SYSTEMS
				// ====================================================================================
				// Data repository
				if (Repository == null && config.Repository != null)
				{
					Repository = config.Repository.ToRepository();
				}
				if (Repository != null)
					Repository.Initialize();
				Log.Info("Initialized repository {0}", Repository);

				// Crypto services
				ICryptoKeyProvider cryptoKeyProvider = null;
				if (!string.IsNullOrEmpty(config.CryptoKeyProviderType))
				{
					var cryptoType = Type.GetType(config.CryptoKeyProviderType, false, true);
					if (cryptoType != null)
						cryptoKeyProvider = Activator.CreateInstance(cryptoType) as ICryptoKeyProvider;
				}
				if (cryptoKeyProvider == null)
					cryptoKeyProvider = new DefaultCryptKeyProvider();
				Cryptography.Initialize(cryptoKeyProvider);
				Log.Info("Initialized the cryptography service.");

				// Connection manager
				var timeout = DefaultConnectionTimeout;
				if (config.ConnectionTimeout > 0)
					timeout = TimeSpan.FromMinutes(config.ConnectionTimeout);
				ConnectionManager.Initialize(timeout);
				Log.Info("Initialized the connection manager.");

				// Game World
				World = new World();
				Log.Info("Initialized the game world.");


				// ====================================================================================
				// INITIALIZE MODULES
				// ====================================================================================
				_modules = LoadedModules.OrderByDescending(m => (int)m.Metadata.Priority).Select(m => m.Value).ToList();
				foreach (var module in _modules)
				{
					module.Initialize();
				}

				// ====================================================================================
				// START THE MAIN GAME LOOP
				// ====================================================================================
				var interval = DefaultGameStepInterval;
				if (config.GameStepInterval > 0)
					interval = TimeSpan.FromMilliseconds(config.GameStepInterval);
				_clock.Start(interval.TotalMilliseconds, Update);
			}
			catch (Exception)
			{
				IsRunning = false;
				throw;
			}
		}

		/// <summary>
		/// Stops the game loop.
		/// </summary>
		public void Stop()
		{
			_clock.Stop();
			IsRunning = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Updates the current game by one tick.
		/// </summary>
		public void Update()
		{
			// Update the game time.
			GameTime.Update(_clock.TotalGameTime);

			// Process input from the network.
			ConnectionManager.CheckInput(); // Handle dead connections and new commands.

			// Process module updates.
			foreach (var module in _modules)
			{
				module.Update(GameTime);
			}

			// Process output to clients.
			ConnectionManager.Flush(); // Send all queued output to the connections.
		}
		#endregion
	}
}