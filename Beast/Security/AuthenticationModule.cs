using System.ComponentModel.Composition;
using Beast.Commands;
using Beast.Net;

namespace Beast.Security
{
	[Export(typeof(IModule))]
	[ExportMetadata("Priority", ModulePriority.High)]
	public class AuthenticationModule : IModule
	{
		public const string LoginCommandName = "login";
		public const string CreateUserCommandName = "createuser";

		public void Initialize()
		{
			CommandManager.Add(LoginCommandName, HandleLoginCommand);
			CommandManager.Add(CreateUserCommandName, HandleCreateUserCommand);
		}

		public void Update(GameTime gameTime)
		{
		}

		private static void HandleLoginCommand(IConnection connection, Command command, CommandMessage response)
		{
		}

		private static void HandleCreateUserCommand(IConnection connection, Command command, CommandMessage response)
		{
		}

		public static bool IsAuthenticationCommand(string commandName)
		{
			return string.Compare(LoginCommandName, commandName, true) == 0 || string.Compare(CreateUserCommandName, commandName, true) == 0;
		}
	}
}
