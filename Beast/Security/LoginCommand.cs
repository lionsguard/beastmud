using System.ComponentModel.Composition;
using Beast.Commands;
using Beast.Net;

namespace Beast.Security
{
	[Export(typeof(ICommand))]
	[ExportMetadata(KeyName, "Login")]
	[ExportMetadata(KeyDescription, "Authenticates user access to the game systems.")]
	[ExportMetadata(KeySynopsis, "LOGIN <username> <password>")]
	[ExportMetadata(KeyAliases, new []{ "login"})]
	[ExportMetadata(KeyArguments, new[] { "username", "password" })]
	public class LoginCommand : Command
	{
		protected override void ExecuteOverride(IInput input, IConnection connection, ResponseMessage response)
		{
			User user;
			if (!Game.Current.Users.TryGetUser(input, out user))
			{
				response.Invalidate(CommonResources.LoginInvalid);
			}

			response.Data = user.Id;
			connection.User = user;
		}
	}
}
