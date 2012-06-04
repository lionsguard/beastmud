
using System.ComponentModel.Composition;
using Beast.Net;

namespace Beast.Commands
{
	[Export(typeof(IModule))]
	[ExportMetadata("Priority", ModulePriority.High)]
	public class StandardCommandsModule: IModule
	{
		public CommandDefinition HelpCommand { get; private set; }
		public CommandDefinition LookCommand { get; private set; }
		public CommandDefinition MoveCommand { get; private set; }

		public void Initialize()
		{
			HelpCommand = Game.Current.Repository.GetCommandDefinition("help");
			MoveCommand = Game.Current.Repository.GetCommandDefinition("movement");
			LookCommand = Game.Current.Repository.GetCommandDefinition("look");

			CommandManager.Add(HelpCommand, Help);
			CommandManager.Add(LookCommand, Look);
			CommandManager.Add(MoveCommand, Move);
		}

		public void Update(GameTime gameTime)
		{
		}

		#region Commands
		private static void Help(IConnection connection, Command command, CommandMessage response)
		{
		}

		private static void Look(IConnection connection, Command command, CommandMessage response)
		{
			
		}
		private static void Move(IConnection connection, Command command, CommandMessage response)
		{

		}
		#endregion
	}
}
