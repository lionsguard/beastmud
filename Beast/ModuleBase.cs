using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast
{
    /// <summary>
    /// Provides an abstract base implementation of the IModule interface.
    /// </summary>
    public abstract class ModuleBase : IModule
    {
        /// <summary>
        /// Gets or sets the Application instance for the current module.
        /// </summary>
        public Application App { get; set; }

        /// <summary>
        /// Gets a value indicating whether or not this module can process the specified input.
        /// </summary>
        /// <param name="input">The IInput to process.</param>
        /// <returns>True if the module processes the specified IInput; otherwise false.</returns>
        public abstract bool CanProcessInput(IO.IInput input);

        /// <summary>
        /// Processes the specified input for the specified connection.
        /// </summary>
        /// <param name="connection">The IConnection instance requesting processing.</param>
        /// <param name="input">The IInput to process.</param>
        public abstract void ProcessInput(IConnection connection, IO.IInput input);

        /// <summary>
        /// Initializes the object.
        /// </summary>
        /// <param name="app">The current Application</param>
        public virtual void Initialize(Application app)
        {
            App = app;
        }

        /// <summary>
        /// Causes the object to clean up resources and shutdown.
        /// </summary>
        public abstract void Shutdown();

        /// <summary>
        /// Updates the current object.
        /// </summary>
        /// <param name="time">The current application time.</param>
        public abstract void Update(ApplicationTime time);
    }
}
