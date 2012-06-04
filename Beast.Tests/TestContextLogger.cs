using System.ComponentModel.Composition;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beast.Tests
{
	[Export(typeof(ILogger))]
	public class TestContextLogger : ILogger
	{
		public static TestContext TestContext;

		#region Implementation of ILogger

		public void Write(LogType type, string message)
		{
			TestContext.WriteLine("[{0}] - {1}", type.ToString().ToUpper(), message);
		}

		#endregion
	}
}