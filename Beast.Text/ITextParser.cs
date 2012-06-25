
using Beast.Net;

namespace Beast.Text
{
	public interface ITextParser
	{
		IInput Parse(string text);
	}
}