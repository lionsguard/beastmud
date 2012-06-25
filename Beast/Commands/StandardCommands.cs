using System.ComponentModel.Composition;
using Beast.Net;

namespace Beast.Commands
{
	#region Help
	[Export(typeof(Command))]
	[ExportMetadata(KeyName, "Help")]
	[ExportMetadata(KeyDescription, "Provides help on the specified command.")]
	[ExportMetadata(KeySynopsis, "HELP <command>")]
	[ExportMetadata(KeyAliases, new[] { "help", "?" })]
	[ExportMetadata(KeyArguments, new[] { "command" })]
	public class HelpCommand : Command
	{
		public override ResponseMessage Execute(IInput input, IConnection connection)
		{
			var commandName = input.Get<string>("command");
			var cmd = Game.Current.Commands.FindCommandInternal(commandName);
			if (cmd == null)
			{
				return new ResponseMessage(input).Invalidate(CommonResources.HelpNotFoundFormat, commandName);
			}

			connection.Write(NotificationMessage.Heading(CommonResources.HelpName), NotificationMessage.Normal(cmd.Metadata.Name));
			connection.Write(NotificationMessage.Heading(CommonResources.HelpDescription), NotificationMessage.Normal(cmd.Metadata.Description));
			connection.Write(NotificationMessage.Heading(CommonResources.HelpSynopsis), NotificationMessage.Normal(cmd.Metadata.Synopsis));
			connection.Write(NotificationMessage.Heading(CommonResources.HelpAliases), NotificationMessage.Normal(string.Join(",", cmd.Metadata.Aliases)));
			return new ResponseMessage(input);
		}
	}
	#endregion
}
