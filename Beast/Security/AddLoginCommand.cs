using System.ComponentModel.Composition;
using System.Linq;
using Beast.Commands;
using Beast.Net;

namespace Beast.Security
{
	[Export(typeof(ICommand))]
	[ExportMetadata(KeyName, "AddLogin")]
	[ExportMetadata(KeyDescription, "Adds new login data to the current user.")]
	[ExportMetadata(KeySynopsis, "ADDLOGIN <username> <password>")]
	[ExportMetadata(KeyAliases, new[] {"addlogin"})]
	[ExportMetadata(KeyArguments, new[]{"username"})]
	public class AddLoginCommand : Command
	{	
		protected override void ExecuteOverride(IInput input, IConnection connection, ResponseMessage response)
		{
			if (connection.User == null)
			{
				response.Invalidate(CommonResources.LoginRequired);
				return;
			}

			var username = input.Get<string>("username");
			var login = connection.User.Logins.FirstOrDefault(l => l.UserName.ToLower() == username);
			if (login != null)
			{
				response.Invalidate(CommonResources.LoginAlreadyExists);
				return;
			}

			if (!Game.Current.Users.TryAddLogin(input, out login))
			{
				response.Invalidate(CommonResources.LoginAlreadyExists);
				return;
			}

			connection.User.Logins.Add(login);
			Game.Current.Repository.SaveUser(connection.User);
		}
	}
}