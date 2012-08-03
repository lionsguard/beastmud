
namespace Beast.Security
{
	public class JanRainLogin : Login
	{
		public const string JanRainTypeIdentifier = "janrain";

		public JanRainLogin()
		{
			TypeIdentifier = JanRainTypeIdentifier;
		}
	}
}
