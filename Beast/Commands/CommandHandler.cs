using Beast.Net;

namespace Beast.Commands
{
	/// <summary>
	/// Defines a delegate for handling the execution of a command.
	/// </summary>
	/// <param name="connection">The Character instance executing the command.</param>
	/// <param name="command">The Command instance containing the arguments for the command.</param>
	/// <param name="response">The response to the execution of the command, pre-initialized for the command.</param>
	public delegate void CommandHandler(IConnection connection, Command command, CommandMessage response);
}
