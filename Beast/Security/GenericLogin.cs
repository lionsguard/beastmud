namespace Beast.Security
{
	public class GenericLogin : Login
	{
		public string Password { get; set; }
		public string PasswordSalt { get; set; }
	}
}