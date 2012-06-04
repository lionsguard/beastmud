using System.ComponentModel.Composition;
using Beast.Commands;
using Beast.Net;

namespace Beast.Security
{
	[Export(typeof(IModule))]
	[ExportMetadata("Priority", ModulePriority.High)]
	public class AuthenticationModule : IModule
	{
		public static CommandDefinition LoginCommand { get; private set; }
		public static CommandDefinition CreateUserCommand { get; private set; }

		public void Initialize()
		{
			LoginCommand = Game.Current.Repository.GetCommandDefinition("login");
			CreateUserCommand = Game.Current.Repository.GetCommandDefinition("createuser");

			CommandManager.Add(LoginCommand, HandleLoginCommand);
			CommandManager.Add(CreateUserCommand, HandleCreateUserCommand);
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
			return string.Compare(LoginCommand, commandName, true) == 0 || string.Compare(CreateUserCommand, commandName, true) == 0;
		}
	}
}
