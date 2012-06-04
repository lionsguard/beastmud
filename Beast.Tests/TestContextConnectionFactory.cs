using Beast.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beast.Tests
{
	public class TestContextConnectionFactory : IConnectionFactory
	{
		public TestContext Context { get; set; }

		public TestContextConnectionFactory(TestContext context)
		{
			Context = context;
		}

		#region Implementation of IConnectionFactory

		public IConnection CreateConnection()
		{
			return new TestContextConnection(Context);
		}

		#endregion
	}
}