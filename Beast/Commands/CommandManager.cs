using System.Collections.Generic;
using System.Linq;
using Beast.Net;

namespace Beast.Commands
{
	/// <summary>
	/// Manages a list of commands and handlers for executing the commands.
	/// </summary>
	public static class CommandManager
	{
		private static readonly Dictionary<CommandDefinition, List<CommandHandler>> Commands = new Dictionary<CommandDefinition, List<CommandHandler>>();

		/// <summary>
		/// Initializes the CommandManager.
		/// </summary>
		internal static void Initialize()
		{
			
		}

		/// <summary>
		/// Gets a CommandDefinition for the specified name or alias.
		/// </summary>
		/// <param name="commandName">The command name or alias.</param>
		/// <returns>An instance of CommandDefinition if found; otherwise null.</returns>
		public static CommandDefinition GetDefinition(string commandName)
		{
			return Commands.Keys.FirstOrDefault(c => c.Aliases.Contains(commandName.ToLower()));
		}

		/// <summary>
		/// Adds a handler to the specified command.
		/// </summary>
		/// <param name="commandDefinition">The definition of the command for which to add a handler.</param>
		/// <param name="handler">The CommandHandler delegate that will process the command.</param>
		public static void Add(CommandDefinition commandDefinition, CommandHandler handler)
		{
			if (!Commands.ContainsKey(commandDefinition))
				Commands[commandDefinition] = new List<CommandHandler>();
			Commands[commandDefinition].Add(handler);
		}

		/// <summary>
		/// Executes the specified command for the specified connection.
		/// </summary>
		/// <param name="command">The Command to execute.</param>
		/// <param name="connection">The IConnection executing the command.</param>
		public static void Execute(Command command, IConnection connection)
		{
			var response = new CommandMessage(command);

			List<CommandHandler> handlers;
			if (!Commands.TryGetValue(command.Name, out handlers))
			{
				response.Invalidate(string.Format(CommonResources.InvalidCommandFormat, command.Name));
			}
			else
			{
				foreach (var handler in handlers)
				{
					handler(connection, command, response);
					if (!response.Success)
						break;
				}
			}

			connection.Write(response);
		}

		/// <summary>
		/// Writes an error to the connection containing details about the command and the required arguments.
		/// </summary>
		/// <param name="connection">The IConnection executing the command.</param>
		/// <param name="command">The Command in question.</param>
		public static void InvalidArguments(IConnection connection, Command command)
		{
			var def = GetDefinition(command.Name);
			if (def == null)
			{
				connection.Write(NotificationMessage.Error(string.Format(CommonResources.InvalidCommandFormat, command.Name)));
				return;
			}

			connection.Write(NotificationMessage.Error(string.Format(CommonResources.CommandInvalidArgumentsFormat, command.Name.ToUpper(), string.Join(", ", def.Synopsis))));
		}
	}
}