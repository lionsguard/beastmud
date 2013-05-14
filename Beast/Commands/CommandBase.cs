using Beast.IO;
using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Beast.Commands
{
    /// <summary>
    /// Provides an abstract base class for commands, implementing a logical workflow for command execution.
    /// </summary>
	public abstract class CommandBase : ICommand 
	{
        /// <summary>
        /// An event that is raised when an error occurs while executing the command.
        /// </summary>
        public event EventHandler<ApplicationErrorEventArgs> Error = delegate { };

        /// <summary>
        /// Gets a list of the argument names for the current command. To work well with text input these should be returned 
        /// in the same order as they are expected to be input.
        /// </summary>
        public abstract IEnumerable<string> ArgumentNames { get; }

        /// <summary>
        /// Executes the current command.
        /// </summary>
        /// <param name="alias">The alias used for executing the command.</param>
        /// <param name="connection">The connection executing the command.</param>
        /// <param name="input">The input for the command.</param>
		public void Execute(string alias, IConnection connection, IInput input)
		{
			var cmdType = GetType().FullName;
			
			try
			{
				Log.Info("Started executing command '{0}'", cmdType);
				
				var output = CreateOutput(alias, input);
				
				if (!IsAuthorized(connection, input))
				{
					Log.Warn("Connection '{0}' not authorized for the command '{1}'", connection.Id, cmdType);
					OnNotAuthorized(output);
					return;
				}
				
				if (!ValidateArguments(input))
				{
					Log.Warn("Invalid arguments for the command '{0}'", cmdType);
					OnInvalidArguments(output);
					return;
				}
				
				Log.Info("Performing execution override");
                ExecuteOverride(connection, input, output);
				
				Log.Info("Writing output to connection");
				connection.Write(output);
				
				Log.Info("Finished executing command '{0}'", cmdType);
			}
			catch (Exception ex)
			{
				Log.Error("Error executing command '{0}': {1}", cmdType, ex);
				OnError(ex);
			}
		}
		
        /// <summary>
        /// Creates an output instance for the current command.
        /// </summary>
        /// <param name="alias">The alias of the current command.</param>
        /// <param name="input">The input for which to create output.</param>
        /// <returns>An IOutput instance for the current command.</returns>
        protected virtual IOutput CreateOutput(string alias, IInput input)
        {
            return new BasicOutput(input.Id) { Command = alias };
        }

        /// <summary>
        /// In a derived class, executes the actual command, updating the specified output as required. The specified output is 
        /// written to the connection by the calling method.
        /// </summary>
        /// <param name="connection">The connection executing the command.</param>
        /// <param name="input">The input for the command.</param>
        /// <param name="output">The output for the current command.</param>
		protected abstract void ExecuteOverride(IConnection connection, IInput input, IOutput output);
		
        /// <summary>
        /// Validates the arguments for the command, checking for number of arguments and data types. Default implementation is empty.
        /// </summary>
        /// <param name="input">The specified input to validate.</param>
        /// <returns>True if the input is valid; otherwise false. Default is true.</returns>
		protected virtual bool ValidateArguments(IInput input)
		{
			return true;
		}
		
        /// <summary>
        /// Checks the current connection and input to determine if the executor is authorized to process the command. Default implementation is empty. 
        /// </summary>
        /// <param name="connection">The connection attempting to execute the command.</param>
        /// <param name="input">The input specified for the command.</param>
        /// <returns>True if the connection is authorized to execute the current command; otherwise false. The default is true.</returns>
		protected virtual bool IsAuthorized(IConnection connection, IInput input)
		{
			return true;
		}
		
        /// <summary>
        /// In a derived class, allows processing an invalid argument message or other process on the current output.
        /// </summary>
        /// <param name="output">The output for the current command.</param>
		protected virtual void OnInvalidArguments(IOutput output)
		{
		}
		
        /// <summary>
        /// In a derived class, allows processing an access denied message or other process on the current output.
        /// </summary>
        /// <param name="output">The output for the current command.</param>
		protected virtual void OnNotAuthorized(IOutput output)
		{
		}
		
        /// <summary>
        /// Handles an exception during execution.
        /// </summary>
        /// <param name="exception">The exception raised during execution.</param>
		protected virtual void OnError(Exception exception)
		{
			Error(this, new ApplicationErrorEventArgs(exception));
		}
	}
}
