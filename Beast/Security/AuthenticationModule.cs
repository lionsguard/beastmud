using System;
using System.ComponentModel.Composition;

namespace Beast.Security
{
	[Export(typeof(IModule))]
	[ExportMetadata("Priority", ModulePriority.High)]
	public class AuthenticationModule : IModule
	{
		public void Initialize()
		{
			throw new NotImplementedException();
		}

		public void Update(GameTime gameTime)
		{
		}

		public static bool IsAuthenticationCommand(string commandName)
		{
			throw new NotImplementedException();
			//return string.Compare(LoginCommand, commandName, true) == 0 || string.Compare(CreateUserCommand, commandName, true) == 0;
		}
	}
}
