using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Beast.Net;

namespace Beast.Security
{
	[Export(typeof(ILoginValidator))]
	[Export(typeof(IKnownTypeDefinition))]
	public class JanRainLoginValidator : IKnownTypeDefinition, ILoginValidator
	{
		public IEnumerable<Type> KnownTypes
		{
			get { return new[] {typeof (JanRainLogin)}; }
		}

		public bool ValidateLogin(IInput input, Login login)
		{
			var type = input.Get<string>(Login.PropertyTypeIdentifier);
			if (string.IsNullOrEmpty(type) || string.Compare(type, JanRainLogin.JanRainTypeIdentifier, true) != 0)
				return false;

			if (!(login is JanRainLogin))
				return false;

			var jrLogin = login as JanRainLogin;
			return jrLogin.UserName == input.Get<string>("username");
		}

		public bool CanValidateLogin(Login login)
		{
			return login is JanRainLogin;
		}

		public Login CreateLogin(IInput input)
		{
			var type = input.Get<string>(Login.PropertyTypeIdentifier);
			if (string.IsNullOrEmpty(type) || string.Compare(type, JanRainLogin.JanRainTypeIdentifier, true) != 0)
				return null;
			return new JanRainLogin
			       	{
						UserName = input.Get<string>("username"),
						Email = input.Get<string>("email"),
			       	};
		}

		public void OnLoginSuccess(User user, Login login, IInput input)
		{
		}
	}
}