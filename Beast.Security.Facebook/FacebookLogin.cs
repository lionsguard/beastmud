namespace Beast.Security
{
	public class FacebookLogin : Login
	{
		public const string FacebookTypeIdentifier = "facebook";

		#region FirstName
		public string FirstName
		{	
			get { return Get<string>(PropertyFirstName); }
			set { Set(PropertyFirstName, value); }
		}
		public static readonly string PropertyFirstName = "FirstName";
		#endregion

		#region LastName
		public string LastName
		{	
			get { return Get<string>(PropertyLastName); }
			set { Set(PropertyLastName, value); }
		}
		public static readonly string PropertyLastName = "LastName";
		#endregion

		#region Gender
		public string Gender
		{
			get { return Get<string>(PropertyGender); }
			set { Set(PropertyGender, value); }
		}
		public static readonly string PropertyGender = "Gender";
		#endregion

		#region AccessToken
		public string AccessToken
		{
			get { return Get<string>(PropertyAccessToken); }
			set { Set(PropertyAccessToken, value); }
		}
		public static readonly string PropertyAccessToken = "AccessToken";	
		#endregion
	}	
}
