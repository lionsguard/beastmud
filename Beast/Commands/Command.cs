using System.Linq;
using Beast.Net;

namespace Beast.Commands
{
	/// <summary>
	/// Represents an abstract base class for handling a command.
	/// </summary>
	public abstract class Command
	{
		public const string KeyName = "Name";
		public const string KeyDescription = "Description";
		public const string KeySynopsis = "Synopsis";
		public const string KeyAliases = "Aliases";
		public const string KeyArguments = "Arguments";

		/// <summary>
		/// Executes the current command.
		/// </summary>
		/// <param name="input">The IInput containing the command information to execute.</param>
		/// <param name="connection">The connection associated with the executing of the command.</param>
		/// <returns>A message detailing the results of command execution.</returns>
		public abstract ResponseMessage Execute(IInput input, IConnection connection);

		/// <summary>
		/// Validates the arguments for the command.
		/// </summary>
		/// <param name="input">The IInput containing the arguments for the command.</param>
		/// <param name="errorMessage">An error message detailing the problems with the arguments, null if the arguments are valid.</param>
		/// <returns>True if the arguments are valid for this command; otherwise false.</returns>
		public virtual bool ValidateArguments(IInput input, out string errorMessage)
		{
			var cmd = Game.Current.Commands.FindCommandInternal(input.CommandName);
			if (cmd == null)
			{
				errorMessage = string.Format(CommonResources.HelpNotFoundFormat, input.CommandName);
				return false;
			}

			if (cmd.Metadata.Arguments.Any(arg => !input.Contains(arg)))
			{
				errorMessage = string.Format(CommonResources.CommandInvalidArgumentsFormat, cmd.Metadata.Name, cmd.Metadata.Synopsis);
				return false;
			}

			errorMessage = null;
			return true;
		}
	}
}