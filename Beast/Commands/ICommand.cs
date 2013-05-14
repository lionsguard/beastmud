using Beast.IO;
using System;
using System.Collections.Generic;

namespace Beast.Commands
{
    /// <summary>
    /// Defines an object that processes IInput as a command.
    /// </summary>
	public interface ICommand 
	{
        /// <summary>
        /// An event that is raised when an error occurs while executing the command.
        /// </summary>
        event EventHandler<ApplicationErrorEventArgs> Error;

        /// <summary>
        /// Gets a list of the argument names for the current command. To work well with text input these should be returned 
        /// in the same order as they are expected to be input.
        /// </summary>
        IEnumerable<string> ArgumentNames { get; }

        /// <summary>
        /// Executes the current command.
        /// </summary>
        /// <param name="alias">The alias used for executing the command.</param>
        /// <param name="connection">The connection executing the command.</param>
        /// <param name="input">The input for the command.</param>
        void Execute(string alias, IConnection connection, IInput input);
	}
}
