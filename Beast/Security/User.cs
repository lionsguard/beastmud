using System;
using System.Collections.Generic;

namespace Beast.Security
{
	public class User
	{
		public string Id { get; set; }
		public UserAccessLevel AccessLevel { get; set; }
		public DateTime DateCreated { get; set; }
		public List<Login> Logins { get; set; }

		public User()
		{
			Logins = new List<Login>();
			AccessLevel = UserAccessLevel.Player;
		}
	}
}
