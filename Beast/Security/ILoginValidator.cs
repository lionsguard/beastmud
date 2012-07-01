using Beast.Net;

namespace Beast.Security
{
	public interface ILoginValidator
	{
		bool ValidateLogin(IInput input, Login login);
		bool CanValidateLogin(Login login);
		Login CreateLogin(IInput input);
	}
}