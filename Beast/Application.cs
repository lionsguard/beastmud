using Beast.Commands;
using Beast.IO;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Beast
{
    /// <summary>
    /// Represents the core application that is repsonsible for all processing and handling of game interactions.
    /// </summary>
    public sealed class Application : IDisposable
    {
        /// <summary>
        /// Gets the IHost instance running the current application.
        /// </summary>
        public IHost Host { get; private set; } 

        /// <summary>
        /// Gets the current settings.
        /// </summary>
        public ApplicationSettings Settings { get; private set; }

        /// <summary>
        /// Gets a value indicating whether or not the Application has been started and is running.
        /// </summary>
        public bool IsRunning
        {
            get { return _isRunning; }
        }

        /// <summary>
        /// Gets the current application time.
        /// </summary>
        public ApplicationTime Time { get; private set; }

        #region Events
        /// <summary>
        /// An event that is raised when processing input fails.
        /// </summary>
        public event EventHandler<InputEventArgs> ProcessInputFailed = delegate { };

        /// <summary>
        /// An event that is raised when a module could not be found to process the specified input.
        /// </summary>
        public event EventHandler<InputEventArgs> ProcessInputModuleNotFound = delegate { };

        /// <summary>
        /// An event that is raised when a new connection is added.
        /// </summary>
        public event EventHandler<ConnectionEventArgs> ConnectionAdded = delegate { };

        /// <summary>
        /// An event that is raised when a new connection is removed.
        /// </summary>
        public event EventHandler<ConnectionEventArgs> ConnectionRemoved = delegate { };
		
        /// <summary>
        /// An event that is raised when an error occurs within the application.
        /// </summary>
		public event EventHandler<ApplicationErrorEventArgs> Error = delegate{};

        /// <summary>
        /// An event that is raised when a command name could not be determined from an IInput instance.
        /// </summary>
        public event EventHandler<InputEventArgs> CommandNameNotFound = delegate { };

        /// <summary>
        /// An event that is raised when a command cannot be located from an IInput instance.
        /// </summary>
        public event EventHandler<CommandNotFoundEventArgs> CommandNotFound = delegate { };
        #endregion

        private readonly ComponentContainer _components;
		private CommandManager _commands;
        private ConnectionManager _connections;
        private CompositionContainer _container;
        private System.Timers.Timer _timer;
        private volatile bool _isRunning;

        [ImportMany]
        private IEnumerable<ILogContext> LoggingContexts { get; set; }

        /// <summary>
        /// Initializes a new instance of the Application class.
        /// </summary>
        /// <param name="host">The current IHost that will run the application.</param>
        /// <param name="settings">The settings for the current application.</param>
        public Application(IHost host, ApplicationSettings settings)
        {
            Host = host;
            Settings = settings;

            Time = new ApplicationTime();

            _components = new ComponentContainer(this);
            _connections = new ConnectionManager(settings.ConnectionTimeout);
			_commands = new CommandManager();
        }

        /// <summary>
        /// Loads external components, intializes modules and starts the main frame loop.
        /// </summary>
        public void Run()
        {
            try
            {
                Log.Initialize();

                Log.Debug("Starting the BeastMUD Application...");

                ComposeParts();

                Log.Load(LoggingContexts);

                Log.Info("Module initialization started");
                _components.Initialize(this);
                Log.Info("Module initialization complete");

                if (_connections == null)
                    _connections = new ConnectionManager(Settings.ConnectionTimeout);

                HookEvents();

                StartMainLoop();
            }
            catch (Exception ex)
            {
                OnError(this, new ApplicationErrorEventArgs(ex));
            }
            
        }

        /// <summary>
        /// Shuts down the main frame loop and all modules and frees system resources.
        /// </summary>
        public void Dispose()
        {
            try
            {
                Log.Info("Stopping the BeastMUD Application...");

                StopMainLoop();

                UnHookEvents();

                _connections.Dispose();
                _connections = null;

                Log.Info("Modules shutdown started");
                _components.Shutdown();
                Log.Info("Modules shutdown complete");

                if (_container != null)
                {
                    _container.Dispose();
                    _container = null;
                }
            }
            catch (Exception ex)
            {
                OnError(this, new ApplicationErrorEventArgs(ex));
            }
            
        }
		
		private void OnError(object sender, ApplicationErrorEventArgs e)
		{
			Error(sender, e);
		}
		
		#region Commands
        /// <summary>
        /// Executes a command for the specified input.
        /// </summary>
        /// <param name="connection">The connection requesting the execution of the command.</param>
        /// <param name="input">The input to process as a command.</param>
        public void ExecuteCommand(IConnection connection, IInput input)
        {
            try
            {
                var cmdName = GetCommandName(input);
                Log.Info("Attempting to find the '{0}' command for '{1}'", cmdName, connection.Id);
                if (string.IsNullOrEmpty(cmdName))
                {
                    Log.Warn("COMMAND NAME NOT FOUND ON INPUT CommandName:'{0}'", 
                        Settings.GetValue(CommandSettingsKeys.CommandNameKey, CommandSettingsKeys.DefaultCommandNameValue));
                    CommandNameNotFound(this, new InputEventArgs(connection, input));
                    return;
                }

                var cmd = _commands.GetCommand(cmdName);
                if (cmd == null)
                {
                    // Check for a catch all command.
                    cmd = _commands.GetCommand(CommandSettingsKeys.CatchAllCommandName);
                    if (cmd == null)
                    {
                        Log.Warn("COMMAND NOT FOUND '{0}'", cmdName);
                        CommandNotFound(this, new CommandNotFoundEventArgs(cmdName, connection, input));
                        return;
                    }
                }

                Log.Info("Executing the '{0}' command for '{1}'", cmdName, connection.Id);
                cmd.Execute(cmdName, connection, input);
            }
            catch (Exception ex)
            {
                OnError(this, new ApplicationErrorEventArgs(ex));
            }
        }

        /// <summary>
        /// Gets an ICommand instance for the specified commandName.
        /// </summary>
        /// <param name="commandName">The name or alias of the command to find.</param>
        /// <returns>An ICommand instance or null.</returns>
        public ICommand GetCommand(string commandName)
        {
            return _commands.GetCommand(commandName);
        }

        /// <summary>
        /// Gets the command name from the specified input.
        /// </summary>
        /// <param name="input">The input containing the command name.</param>
        /// <returns>The command name if found; otherwise null.</returns>
        public string GetCommandName(IInput input)
        {
            return input.Get<string>(Settings.GetValue(CommandSettingsKeys.CommandNameKey, CommandSettingsKeys.DefaultCommandNameValue), string.Empty);
        }
		#endregion

        #region Connections
        /// <summary>
        /// Adds the specified connection to the Application.
        /// </summary>
        /// <param name="connection">The connection to add.</param>
        public void AddConnection(IConnection connection)
        {
            connection.LastActivityTick = Time.Ticks;
            _connections.Add(connection);
        }

        /// <summary>
        /// Attempts to find an IConnection instance for the current id.
        /// </summary>
        /// <param name="id">The id of the connection to find.</param>
        /// <returns>An IConnection instance with the specified id value or null if not found.</returns>
        public IConnection FindConnection(string id)
        {
            return _connections.Get(id);
        }

        private void OnConnectionAdded(object sender, ConnectionEventArgs e)
        {
            Log.Info("Connection Added '{0}'", e.Connection.Id);
            ConnectionAdded(this, e);
        }
        private void OnConnectionRemoved(object sender, ConnectionEventArgs e)
        {
            Log.Info("Connection Removed '{0}'", e.Connection.Id);
            ConnectionRemoved(this, e);
        }
        #endregion

        #region ProcessInput
        /// <summary>
        /// Processes input for the specified connection.
        /// </summary>
        /// <param name="connection">The connection requesting processing of the specified input.</param>
        /// <param name="input">The input to process.</param>
        public void ProcessInput(IConnection connection, IInput input)
        {
            Task.Run(() =>
                {
                    Log.Info("Processing input for connection '{0}'", connection.Id);

                    try
                    {
                        var processed = 0;
                        foreach (var module in _components.Modules)
                        {
                            if (module.CanProcessInput(input))
                            {
                                processed++;
                                try
                                {
                                    connection.LastActivityTick = Time.Ticks;
                                    module.ProcessInput(connection, input);
                                }
                                catch (Exception ex)
                                {
                                    Log.Error("ERROR PROCESSING INPUT ON MODULE '{0}': {1}", module, ex);
                                    ProcessInputFailed(this, new InputEventArgs(connection, input));
                                }
                            }
                        }

                        if (processed == 0)
                        {
                            Log.Warn("NO MODULES FOUND TO PROCESS '{0}'", input);
                            ProcessInputModuleNotFound(this, new InputEventArgs(connection, input));
                            connection.Write(input.CreateOutput().Invalidate("The specified input was not understood or processed by the application."));
                        }
                    }
                    catch (Exception e)
                    {
                        OnError(this, new ApplicationErrorEventArgs(e));
                    }
                });
        }
        #endregion

        #region Broadcast
        /// <summary>
        /// Broadcasts the specified output to all connections.
        /// </summary>
        /// <param name="output">The output to broadcast.</param>
        public void Broadcast(IOutput output)
        {
            try
            {
                _connections.Broadcast(output);
            }
            catch (Exception ex)
            {
                OnError(this, new ApplicationErrorEventArgs(ex));
            }
        }
        #endregion

        #region ComposeParts
        private void ComposeParts()
        {
            try
            {
                var catalog = new AggregateCatalog(new AssemblyCatalog(typeof(Application).Assembly));

                foreach (var asm in Settings.ComponentAssemblies)
                {
                    Log.Info("COMPOSITION: Adding assembly '{0}'", asm.FullName);
                    catalog.Catalogs.Add(new AssemblyCatalog(asm));
                }

                foreach (var dir in Settings.ComponentDirectories)
                {
                    var cat = new DirectoryCatalog(dir);
                    Log.Info("COMPOSITION: Adding directory '{0}'", cat.Path);
                    catalog.Catalogs.Add(cat);
                }

                _container = new CompositionContainer(catalog);

                // Compose the modules, initializables and updatables.
                Log.Info("COMPOSITION: Composing Application");
                _container.ComposeParts(this, _components, _commands);
            }
            catch (Exception ex)
            {
                OnError(this, new ApplicationErrorEventArgs(ex));
            }
        }
        #endregion

        #region Update
        private void Update()
        {
            Log.Debug("Server update started");
            Time.Update();
            try
            {
                _components.Update(Time);
            }
            catch (Exception ex)
            {
                OnError(this, new ApplicationErrorEventArgs(ex));
            }
            Log.Debug("Server update complete. Time: {0}", Time.Elapsed);
        }
        #endregion

        #region Main Loop
        private void StartMainLoop()
        {
            _isRunning = true;
            Time = new ApplicationTime();

            Log.Info("Calling Update for the first time");
            Update();

            Log.Info("Starting main loop with an interval of '{0}'", Settings.UpdateInterval);
            _timer = new System.Timers.Timer(Settings.UpdateInterval.TotalMilliseconds);
            _timer.Elapsed += (o, e) => Update();
            _timer.Start();
        }
        private void StopMainLoop()
        {
            _isRunning = false;

            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
            }

            if (Time != null)
                Time.Dispose();
        }
        #endregion

        #region Hook/Unhook Events
        private void HookEvents()
        {
            _connections.ConnectionAdded += OnConnectionAdded;
            _connections.ConnectionRemoved += OnConnectionRemoved;
			
			_commands.Error += OnError;
        }

        private void UnHookEvents()
        {
            _connections.ConnectionAdded -= OnConnectionAdded;
            _connections.ConnectionRemoved -= OnConnectionRemoved;
			
			_commands.Error -= OnError;
        }
        #endregion

        #region Modules
        /// <summary>
        /// Gets a list of modules currently registered with the application.
        /// </summary>
        /// <returns>A list of modules currently registered with the application.</returns>
        public IEnumerable<IModule> GetModules()
        {
            return _components.Modules;
        }
        #endregion
    }
}
