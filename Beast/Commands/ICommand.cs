using Beast.IO;
using System;

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
        /// Executes the current command.
        /// </summary>
        /// <param name="connection">The connection executing the command.</param>
        /// <param name="input">The input for the command.</param>
        void Execute(IConnection connection, IInput input);
	}
}
