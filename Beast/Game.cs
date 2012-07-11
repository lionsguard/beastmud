using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.IO;
using System.Linq;
using Beast.Commands;
using Beast.Configuration;
using Beast.Data;
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

		[ImportMany(AllowRecomposition = true, RequiredCreationPolicy = CreationPolicy.Shared)]
		private IEnumerable<Lazy<IModule, IModuleMetadata>> LoadedModules { get; set; }

		[ImportMany(typeof(ILogger), AllowRecomposition = true)]
		private IEnumerable<ILogger> Loggers { get; set; }

		[ImportMany(typeof(IKnownTypeDefinition), AllowRecomposition = true)]
		private IEnumerable<IKnownTypeDefinition> TypeResolvers { get; set; }

		[ImportMany(typeof(IGameObjectCreator), AllowRecomposition = true)]
		private IEnumerable<IGameObjectCreator> ObjectCreators { get; set; }

		/// <summary>
		/// Gets or sets the current data repository.
		/// </summary>
		public RepositoryManager Repository { get; set; }

		/// <summary>
		/// Gets a value indicating whether or not the game is currently running.
		/// </summary>
		public bool IsRunning { get; private set; }

		/// <summary>
		/// Gets the current world instance.
		/// </summary>
		public IWorld World { get; private set; }

		/// <summary>
		/// Gets the current GameTime.
		/// </summary>
		public GameTime GameTime { get; private set; }

		/// <summary>
		/// Gets the currently loaded configuration section.
		/// </summary>
		public BeastSection Config { get; private set; }

		/// <summary>
		/// Gets the current CommandManager instance.
		/// </summary>
		public CommandManager Commands { get; private set; }

		/// <summary>
		/// Gets an object used to validate users and process login information.
		/// </summary>
		public UserManager Users { get; private set; }

		private GameClock _clock;

		private List<IModule> _modules = new List<IModule>();

		private Game()
		{
			Repository= new RepositoryManager();
			Commands = new CommandManager();
			Users = new UserManager();
		}

		#region Start and Stop
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
						modulesDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, config.ModulesDirectory.Path);
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

				var container = new CompositionContainer(catalog, new ConfigurationExportProvider(new FileConfigurationSource()));
				container.ComposeParts(this, Repository, Commands, Users);

				// ====================================================================================
				// INITIALIZE LOGGING
				// ====================================================================================
				Log.Initialize(Loggers);

				// ====================================================================================
				// INITIALIZE BASIC SYSTEMS
				// ====================================================================================
				// Data repository
				Repository.Initialize();
				Log.Info("Initialized repositories.");

				// Crypto services
				ICryptoKeyProvider cryptoKeyProvider = null;
				if (!string.IsNullOrEmpty(config.CryptoKeyProviderType))
				{
					var cryptoType = FindType(config.CryptoKeyProviderType);
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
				World = Repository.GetWorld() ?? new DefaultWorld();
				World.Initialize();
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
			Repository.Shutdown();
			foreach (var module in _modules)
			{
				module.Shutdown();
			}
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

		#region Resolve Types

		private readonly List<Type> _resolvedTypes = new List<Type>();
		private void EnsureResolvedTypes()
		{
			if (_resolvedTypes.Count != 0) 
				return;
			
			lock (_resolvedTypes)
			{
				foreach (var resolver in TypeResolvers)
				{
					_resolvedTypes.AddRange(resolver.KnownTypes);
				}
			}
		}

		/// <summary>
		/// Attempts to find a System.Type with the specified name using the current TypeResolvers.
		/// </summary>
		/// <param name="name">The name of the Type to find.</param>
		/// <returns>The System.Type with the specified name or null if not found.</returns>
		public Type FindType(string name)
		{
			EnsureResolvedTypes();
			return _resolvedTypes.FirstOrDefault(t => t.Name.ToLower() == name.ToLower());
		}

		/// <summary>
		/// Attempts to find all types that derive from the specified baseType.
		/// </summary>
		/// <param name="baseType">The base type.</param>
		/// <returns>A list of types that derive from the specified base type.</returns>
		public IEnumerable<Type> FindTypes(Type baseType)
		{
			EnsureResolvedTypes();
			return _resolvedTypes.Where(baseType.IsAssignableFrom);
		}

		public IEnumerable<Type> GetKnownTypes()
		{
			EnsureResolvedTypes();
			return _resolvedTypes;
		}
		#endregion

		#region Create Objects
		public bool TryCreateObject(Type type, IInput input, out string errorMessage, out IGameObject obj)
		{
			var creator = ObjectCreators.FirstOrDefault(c => c.CanCreate(type));
			if (creator != null)
			{
				return creator.TryCreate(type, input, out errorMessage, out obj);
			}
			errorMessage = string.Format(CommonResources.GameObjectCreatorNotFoundFormat, type.FullName);
			obj = null;
			return false;
		}
		#endregion
	}
}