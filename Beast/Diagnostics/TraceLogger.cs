using System.ComponentModel.Composition;
using System.Diagnostics;

namespace Beast.Diagnostics
{
	[Export(typeof(ILogger))]
	public class TraceLogger : ILogger
	{
		public void Write(LogType type, string message)
		{
			switch (type)
			{
				case LogType.Debug:
					Trace.WriteLine(string.Format("[{0}] - {1}", type, message));
					break;
				case LogType.Info:
					Trace.TraceInformation(message);
					break;
				case LogType.Warn:
					Trace.TraceWarning(message);
					break;
				case LogType.Error:
					Trace.TraceWarning(message);
					break;
			}
		}
	}
}
