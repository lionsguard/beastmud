using System.ComponentModel.Composition;
using Beast.Commands;
using Beast.Net;

namespace Beast.Security
{
	[Export(typeof(ICommand))]
	[ExportMetadata(KeyName, "CreateUser")]
	[ExportMetadata(KeyDescription, "Creates a new user account and associates one type of login.")]
	[ExportMetadata(KeySynopsis, "CREATEUSER <username> <password>")]
	[ExportMetadata(KeyAliases, new []{"createuser"})]
	[ExportMetadata(KeyArguments, new string[0])]
	public class CreateUserCommand : Command
	{
		protected override void ExecuteOverride(IInput input, IConnection connection, ResponseMessage response)
		{
			User user;
			if (Game.Current.Users.TryGetUser(input, out user))
			{
				// User already exits.
				response.Invalidate(CommonResources.LoginAlreadyExists);
				return;
			}

			// Otherwise, create a user account and add a login.
			user = new User();
			Login login;
			if (!Game.Current.Users.TryAddLogin(input, out login))
			{
				// User already exits.
				response.Invalidate(CommonResources.LoginAlreadyExists);
				return;
			}

			user.Logins.Add(login);
			Game.Current.Repository.SaveUser(user);
			connection.User = user;
		}
	}
}
