﻿using Beast.IO;
using System;
using System.Collections.Concurrent;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
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
        public event EventHandler<ProcessInputEventArgs> ProcessInputFailed = delegate { };

        /// <summary>
        /// An event that is raised when a module could not be found to process the specified input.
        /// </summary>
        public event EventHandler<ProcessInputEventArgs> ProcessInputModuleNotFound = delegate { };

        /// <summary>
        /// An event that is raised when a new connection is added.
        /// </summary>
        public event EventHandler<ConnectionEventArgs> ConnectionAdded = delegate { };

        /// <summary>
        /// An event that is raised when a new connection is removed.
        /// </summary>
        public event EventHandler<ConnectionEventArgs> ConnectionRemoved = delegate { };
        #endregion

        private readonly ComponentContainer _components = new ComponentContainer();
        private ConnectionManager _connections;
        private CompositionContainer _container;
        private System.Timers.Timer _timer;
        private volatile bool _isRunning;

        /// <summary>
        /// Initializes a new instance of the Application class.
        /// </summary>
        /// <param name="host">The current IHost that will run the application.</param>
        /// <param name="settings">The settings for the current application.</param>
        public Application(IHost host, ApplicationSettings settings)
        {
            Host = host;
            Settings = settings;

            _connections = new ConnectionManager(settings.ConnectionTimeout);
        }

        /// <summary>
        /// Loads external components, intializes modules and starts the main frame loop.
        /// </summary>
        public void Run()
        {
            Trace.TraceInformation("Starting the BeastMUD Application...");

            ComposeParts();

            Trace.TraceInformation("Module initialization started");
            _components.Initialize();
            Trace.TraceInformation("Module initialization complete");

            if (_connections == null)
                _connections = new ConnectionManager(Settings.ConnectionTimeout);

            HookEvents();

            StartMainLoop();
        }

        /// <summary>
        /// Shuts down the main frame loop and all modules and frees system resources.
        /// </summary>
        public void Dispose()
        {
            Trace.TraceInformation("Stopping the BeastMUD Application...");

            StopMainLoop();

            UnHookEvents();

            _connections.Dispose();
            _connections = null;

            Trace.TraceInformation("Modules shutdown started");
            _components.Shutdown();
            Trace.TraceInformation("Modules shutdown complete");

            if (_container != null)
            {
                _container.Dispose();
                _container = null;
            }
        }

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
            Trace.TraceInformation("Connection Added '{0}'", e.Connection.Id);
            ConnectionAdded(this, e);
        }
        private void OnConnectionRemoved(object sender, ConnectionEventArgs e)
        {
            Trace.TraceInformation("Connection Removed '{0}'", e.Connection.Id);
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
                    Trace.TraceInformation("Processing input for connection '{0}'", connection.Id);

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
                                Trace.TraceError("ERROR PROCESSING INPUT ON MODULE '{0}': {1}", module, ex);
                                ProcessInputFailed(this, new ProcessInputEventArgs(connection, input));
                            }
                        }
                    }

                    if (processed == 0)
                    {
                        Trace.TraceWarning("NO MODULES FOUND TO PROCESS '{0}'", input);
                        ProcessInputModuleNotFound(this, new ProcessInputEventArgs(connection, input));
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
            _connections.Broadcast(output);
        }
        #endregion

        #region ComposeParts
        private void ComposeParts()
        {
            var catalog = new AggregateCatalog(new AssemblyCatalog(typeof(Application).Assembly));

            foreach (var asm in Settings.ComponentAssemblies)
            {
                Trace.TraceInformation("COMPOSITION: Adding assembly '{0}'", asm.FullName);
                catalog.Catalogs.Add(new AssemblyCatalog(asm));
            }

            foreach (var dir in Settings.ComponentDirectories)
            {
                var cat = new DirectoryCatalog(dir);
                Trace.TraceInformation("COMPOSITION: Adding directory '{0}'", cat.Path);
                catalog.Catalogs.Add(cat);
            }

            _container = new CompositionContainer(catalog);

            // Compose the modules, initializables and updatables.
            Trace.TraceInformation("COMPOSITION: Composing Application");
            _container.ComposeParts(this, _components);

            // Compose the module instances themselves to allow modules to have parts.
            Trace.TraceInformation("COMPOSITION: Composing Modules");
            _components.Compose(this, _container);
        }
        #endregion

        #region Update
        private void Update()
        {
            Trace.TraceInformation("Server update started");
            Time.Update();
            _components.Update(Time);
            Trace.TraceInformation("Server update complete. Time: {0}", Time.Elapsed);
        }
        #endregion

        #region Main Loop
        private void StartMainLoop()
        {
            _isRunning = true;
            Time = new ApplicationTime();

            Trace.TraceInformation("Calling Update for the first time");
            Update();

            Trace.TraceInformation("Starting main loop with an interval of '{0}'", Settings.UpdateInterval);
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
        }

        private void UnHookEvents()
        {
            _connections.ConnectionAdded -= OnConnectionAdded;
            _connections.ConnectionRemoved -= OnConnectionRemoved;
        }
        #endregion
    }
}
