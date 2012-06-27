using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Beast.Net;

namespace Beast.Commands
{
	/// <summary>
	/// Manages a list of commands and handlers for executing the commands.
	/// </summary>
	public class CommandManager
	{
		/// <summary>
		/// Gets a list of all loaded commands.
		/// </summary>
		[ImportMany(typeof(ICommand), AllowRecomposition = true)]
		public IEnumerable<Lazy<ICommand, ICommandMetadata>> Commands { get; private set; }

		internal CommandManager()
		{
		}

		internal Lazy<ICommand, ICommandMetadata> FindCommandInternal(string name)
		{
			return Commands.FirstOrDefault(c => string.Compare(c.Metadata.Name, name, true) == 0 || c.Metadata.Aliases.Any(a => string.Compare(a, name, true) == 0));
		}

		/// <summary>
		/// Finds a command by matching the specified name to the command metadata name or aliases.
		/// </summary>
		/// <param name="name">The name or alias of the command to find.</param>
		/// <returns>An ICommandMetadata instance or null of the command was not found.</returns>
		public ICommandMetadata FindCommand(string name)
		{
			var cmd = FindCommandInternal(name);
			return cmd == null ? null : cmd.Metadata;
		}

		/// <summary>
		/// Executes the specified command for the specified connection.
		/// </summary>
		/// <param name="input">The IInput containing the command information to execute.</param>
		/// <param name="connection">The IConnection executing the command.</param>
		public void Execute(IInput input, IConnection connection)
		{
			// Find the command from the input.
			var cmd = FindCommandInternal(input.CommandName);
			if (cmd == null)
			{
				InvalidateResponse(input, connection, CommonResources.CommandInvalidFormat, input.CommandName);
				return;
			}

			string errorMessage;
			if (!cmd.Value.ValidateArguments(input, out errorMessage))
			{
				InvalidateResponse(input, connection, errorMessage);
				return;
			}

			var response = cmd.Value.Execute(input, connection);
			connection.Write(response);
		}

		private static void InvalidateResponse(IInput input, IConnection connection, string format, params object[] args)
		{
			var response = new ResponseMessage(input);
			response.Invalidate(format, args);
			connection.Write(response);
		}
	}
}