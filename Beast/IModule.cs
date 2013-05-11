using Beast.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast
{
    /// <summary>
    /// Defines an object that will participate in MEF composition and can provide application or sub system functionality.
    /// </summary>
    public interface IModule : IInitializable, IUpdatable
    {
        /// <summary>
        /// Gets or sets the Application instance for the current module.
        /// </summary>
        Application App { get; set; }

        /// <summary>
        /// Gets a value indicating whether or not this module can process the specified input.
        /// </summary>
        /// <param name="input">The IInput to process.</param>
        /// <returns>True if the module processes the specified IInput; otherwise false.</returns>
        bool CanProcessInput(IInput input);

        /// <summary>
        /// Processes the specified input for the specified connection.
        /// </summary>
        /// <param name="connection">The IConnection instance requesting processing.</param>
        /// <param name="input">The IInput to process.</param>
        void ProcessInput(IConnection connection, IInput input);
    }
}
