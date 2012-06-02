using System;
using System.Collections.Generic;
using Beast.Net;

namespace Beast.Commands
{
	/// <summary>
	/// Manages a list of commands and handlers for executing the commands.
	/// </summary>
	public static class CommandManager
	{
		private static readonly Dictionary<string, List<CommandHandler>> Commands = new Dictionary<string, List<CommandHandler>>(StringComparer.InvariantCultureIgnoreCase);

		/// <summary>
		/// Adds a handler to the specified command.
		/// </summary>
		/// <param name="commandName">The name of the command for which to add a handler.</param>
		/// <param name="handler">The CommandHandler delegate that will process the command.</param>
		public static void Add(string commandName, CommandHandler handler)
		{
			if (!Commands.ContainsKey(commandName))
				Commands[commandName] = new List<CommandHandler>();
			Commands[commandName].Add(handler);
		}

		/// <summary>
		/// Executes the specified command for the specified connection.
		/// </summary>
		/// <param name="command">The Command to execute.</param>
		/// <param name="connection">The IConnection executing the command.</param>
		/// <returns>An instance of CommandMessage containing the response to the command.</returns>
		public static CommandMessage Execute(Command command, IConnection connection)
		{
			var response = new CommandMessage(command);

			List<CommandHandler> handlers;
			if (!Commands.TryGetValue(command.Name, out handlers))
			{
				return response.Invalidate(string.Format(CommonResources.InvalidCommandFormat, command.Name));
			}

			foreach (var handler in handlers)
			{
				handler(connection, command, response);
				if (!response.Success)
					break;
			}

			return response;
		}
	}
}