using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Beast.Net;
using Facebook;

namespace Beast.Security
{
	[Export(typeof(ILoginValidator))]
	[Export(typeof(IKnownTypeDefinition))]
	public class FacebookLoginValidator : IKnownTypeDefinition, ILoginValidator
	{
		#region Implementation of ILoginValidator

		public bool ValidateLogin(IInput input, Login login)
		{
			var type = input.Get<string>(Login.PropertyTypeIdentifier);
			if (string.IsNullOrEmpty(type) || string.Compare(type, FacebookLogin.FacebookTypeIdentifier, true) != 0)
				return false;

			if (string.IsNullOrEmpty(input.Get<string>("username")) || !(login is FacebookLogin))
				return false;
			var client = new FacebookClient(input.Get<string>("access_token"));
			dynamic me = client.Get("me", new {fields = "id"});

			return me.id == input.Get<string>("username");
		}

		public bool CanValidateLogin(Login login)
		{
			return login is FacebookLogin;
		}

		public Login CreateLogin(IInput input)
		{
			var type = input.Get<string>(Login.PropertyTypeIdentifier);
			if (string.IsNullOrEmpty(type) || string.Compare(type, FacebookLogin.FacebookTypeIdentifier, true) != 0)
				return null;
			return new FacebookLogin
			       	{
						UserName = input.Get<string>("username"),
						FirstName = input.Get<string>("first_name"),
						LastName = input.Get<string>("last_name"),
						Gender = input.Get<string>("gender"),
						AccessToken = input.Get<string>("access_token"),
						TypeIdentifier = type
			       	};
		}

		public void OnLoginSuccess(User user, Login login, IInput input)
		{
			var fbLogin = login as FacebookLogin;
			if (fbLogin == null)
				return;

			fbLogin.AccessToken = input.Get<string>("access_token");
		}

		#endregion

		public IEnumerable<Type> KnownTypes
		{
			get { return new[] {typeof (FacebookLogin)}; }
		}
	}
}