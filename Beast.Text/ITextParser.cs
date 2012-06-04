
using Beast.Commands;

namespace Beast.Text
{
	public interface ITextParser
	{
		Command Parse(string text);
	}
}