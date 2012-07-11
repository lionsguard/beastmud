namespace Beast.Security
{
	public class GenericLogin : Login
	{
		#region Password
		public string Password
		{	
			get { return Get<string>(PropertyPassword); }
			set { Set(PropertyPassword, value); }
		}
		public static readonly string PropertyPassword = "Password";
		#endregion

		#region PasswordSalt
		public string PasswordSalt
		{	
			get { return Get<string>(PropertyPasswordSalt); }
			set { Set(PropertyPasswordSalt, value); }
		}
		public static readonly string PropertyPasswordSalt = "PasswordSalt";
		#endregion
	}
}