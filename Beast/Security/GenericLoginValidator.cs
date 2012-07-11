using System.ComponentModel.Composition;
using Beast.Net;

namespace Beast.Security
{
	[Export(typeof(ILoginValidator))]
	public class GenericLoginValidator : ILoginValidator
	{
		public bool ValidateLogin(IInput input, Login login)
		{
			var password = input.Get<string>("password");
			if (!string.IsNullOrEmpty(password))
			{
				var genLogin = login as GenericLogin;
				if (genLogin != null)
				{
					var hashedPwd = Cryptography.ComputeHash(password, genLogin.PasswordSalt);
					if (hashedPwd == genLogin.Password)
					{
						// Valid login.
						return true;
					}
				}
			}
			return false;
		}

		public bool CanValidateLogin(Login login)
		{
			return login is GenericLogin;
		}

		public Login CreateLogin(IInput input)
		{
			var username = input.Get<string>("username");
			var password = input.Get<string>("password");
			var email = input.Get<string>("email");

			if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
				return null;

			var pwdSalt = Cryptography.CreateSalt();
			var hashedPwd = Cryptography.ComputeHash(password, pwdSalt);

			return new GenericLogin
			       	{
			       		UserName = username,
			       		Password = hashedPwd,
						PasswordSalt = pwdSalt,
						Email = email
			       	};
		}

		public void OnLoginSuccess(User user, Login login, IInput input)
		{
			
		}
	}
}