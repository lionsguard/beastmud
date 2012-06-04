using System.Collections.Generic;
using Beast.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Beast.Tests
{
	public class TestContextConnection : ConnectionBase
	{
		public TestContext Context { get; set; }

		public TestContextConnection(TestContext context)
		{
			Context = context;
		}

		#region Overrides of ConnectionBase

		protected override void FlushOverride(IEnumerable<IMessage> messages)
		{
			foreach (var message in messages)
			{
				Context.WriteLine("{0}", JsonConvert.SerializeObject(message));	
			}
		}

		#endregion
	}
}