using System.ComponentModel.Composition;
using System.Linq;
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
			var cmd = command.Get<string>("command");
			if (string.IsNullOrEmpty(cmd))
			{
				response.Invalidate(CommonResources.HelpInvalidArgument);
				return;
			}

			var def = CommandManager.GetDefinition(cmd);
			if (def == null)
			{
				response.Invalidate(string.Format(CommonResources.HelpNotFoundFormat, cmd.ToUpper()));
				return;
			}

			connection.Write(NotificationMessage.Heading(CommonResources.HelpName), 
				NotificationMessage.Normal(def.Name),
				NotificationMessage.Heading(CommonResources.HelpSynopsis));
			connection.Write(def.Synopsis.Select(NotificationMessage.Normal).ToArray());
			connection.Write(NotificationMessage.Heading(CommonResources.HelpDescription),
				NotificationMessage.Normal(def.Description),
				NotificationMessage.Heading(CommonResources.HelpAliases));
			connection.Write(def.Aliases.Select(NotificationMessage.Normal).ToArray());
		}

		private static void Look(IConnection connection, Command command, CommandMessage response)
		{
			// Direction
			var direction = command.Get<string>("direction");
			if (!string.IsNullOrEmpty(direction))
			{
				var dir = Direction.FromNameOrAlias(direction);
				if (dir.Value == KnownDirection.Void)
				{
					connection.Write(NotificationMessage.Error(string.Format(CommonResources.DirectionNotFoundFormat, direction)));
					return;
				}
				return;
			}

			// Object/Entity
			var name = command.Get<string>("name");
			if (!string.IsNullOrEmpty(name))
			{
				return;
			}

			// Place
			if (connection.Character != null)
			{
				var place = Game.Current.World[connection.Character.Position];
				if (place != null)
				{
					place.Look(connection.Character);
					return;
				}
			}

			// Invalid args
			CommandManager.InvalidArguments(connection, command);
		}
		private static void Move(IConnection connection, Command command, CommandMessage response)
		{

		}
		#endregion
	}
}
