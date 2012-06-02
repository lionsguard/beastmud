
namespace Beast
{
	public interface ILogger
	{
		void Write(LogType type, string message);
	}
}