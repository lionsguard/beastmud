using System;

namespace Beast.Security
{
	public abstract class Login : DataObject
	{
		#region UserName
		public string UserName
		{
			get { return Get<string>(PropertyUserName); }
			set { Set(PropertyUserName, value); }
		}
		public static readonly string PropertyUserName = "UserName";
		#endregion

		#region Email
		public string Email
		{	
			get { return Get<string>(PropertyEmail); }
			set { Set(PropertyEmail, value); }
		}
		public static readonly string PropertyEmail = "Email";
		#endregion
		
		#region DateCreated
		public DateTime DateCreated
		{	
			get { return Get<DateTime>(PropertyDateCreated); }
			set { Set(PropertyDateCreated, value); }
		}
		public static readonly string PropertyDateCreated = "DateCreated";
		#endregion

		#region DateLastLogin
		public DateTime DateLastLogin
		{	
			get { return Get<DateTime>(PropertyDateLastLogin); }
			set { Set(PropertyDateLastLogin, value); }
		}
		public static readonly string PropertyDateLastLogin = "DateLastLogin";
		#endregion

		protected Login()
		{
			DateCreated = DateTime.UtcNow;
			DateLastLogin = DateTime.UtcNow;
		}
	}
}