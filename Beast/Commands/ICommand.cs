using Beast.Net;

namespace Beast.Commands
{
	/// <summary>
	/// Represents an abstract base class for handling a command.
	/// </summary>
	public interface ICommand
	{
		/// <summary>
		/// Gets a value indicating whether or not the command requires a valid User.
		/// </summary>
		bool RequiresUser { get; }

		/// <summary>
		/// Gets a value indicating whether or not the command requires a valid Character.
		/// </summary>
		bool RequiresCharacter { get; }

		/// <summary>
		/// Executes the current command.
		/// </summary>
		/// <param name="input">The IInput containing the command information to execute.</param>
		/// <param name="connection">The connection associated with the executing of the command.</param>
		/// <returns>A message detailing the results of command execution.</returns>
		ResponseMessage Execute(IInput input, IConnection connection);

		/// <summary>
		/// Validates the arguments for the command.
		/// </summary>
		/// <param name="input">The IInput containing the arguments for the command.</param>
		/// <param name="errorMessage">An error message detailing the problems with the arguments, null if the arguments are valid.</param>
		/// <returns>True if the arguments are valid for this command; otherwise false.</returns>
		bool ValidateArguments(IInput input, out string errorMessage);
	}
}