using System;

namespace Beast.Security
{
	public abstract class Login
	{
		public string UserName { get; set; }
		public string Email { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateLastLogin { get; set; }

		protected Login()
		{
			DateCreated = DateTime.UtcNow;
			DateLastLogin = DateTime.UtcNow;
		}
	}
}