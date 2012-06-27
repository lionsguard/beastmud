using System.ComponentModel.Composition;
using System.Linq;
using Beast.Net;

namespace Beast.Commands
{
	#region Help
	[Export(typeof(ICommand))]
	[ExportMetadata(KeyName, "Help")]
	[ExportMetadata(KeyDescription, "Provides help on the specified command.")]
	[ExportMetadata(KeySynopsis, "HELP <command>")]
	[ExportMetadata(KeyAliases, new[] { "help", "?" })]
	[ExportMetadata(KeyArguments, new[] { "command" })]
	public class HelpCommand : Command
	{
		protected override void ExecuteOverride(IInput input, IConnection connection, ResponseMessage response)
		{
			var commandName = input.Get<string>("command");
			var cmd = Game.Current.Commands.FindCommandInternal(commandName);
			if (cmd == null)
			{
				response.Invalidate(CommonResources.HelpNotFoundFormat, commandName);
				return;
			}

			connection.Write(NotificationMessage.Heading(CommonResources.HelpName), NotificationMessage.Normal(cmd.Metadata.Name));
			connection.Write(NotificationMessage.Heading(CommonResources.HelpDescription), NotificationMessage.Normal(cmd.Metadata.Description));
			connection.Write(NotificationMessage.Heading(CommonResources.HelpSynopsis), NotificationMessage.Normal(cmd.Metadata.Synopsis));
			connection.Write(NotificationMessage.Heading(CommonResources.HelpAliases), NotificationMessage.Normal(string.Join(",", cmd.Metadata.Aliases)));
		}
	}
	#endregion

	#region Who
	[Export(typeof(ICommand))]
	[ExportMetadata(KeyName, "Who")]
	[ExportMetadata(KeyDescription, "List all players currently online.")]
	[ExportMetadata(KeySynopsis, "WHO")]
	[ExportMetadata(KeyAliases, new[] {"who"})]
	[ExportMetadata(KeyArguments, new[]{""})]
	public class WhoCommand : Command
	{	
		protected override void ExecuteOverride(IInput input, IConnection connection, ResponseMessage response)
		{
			var players = ConnectionManager.ActivePlayers();
			var count = players.Count();
			if (count > 0)
			{
				connection.Write(NotificationMessage.Normal(string.Format(CommonResources.WhoPlayersFormat, count)));
				foreach (var player in players)
				{
					connection.Write(NotificationMessage.Normal(player.ToShortString()));
				}
			}
			else
			{
				connection.Write(NotificationMessage.Normal(CommonResources.WhoNoPlayers));
			}
		}
	}
	#endregion
}
