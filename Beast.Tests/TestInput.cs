using System.Collections.Generic;
using Beast.Net;

namespace Beast.Tests
{
	public class TestInput : InputBase
	{
		public TestInput(string commandName, params KeyValuePair<string, object>[] args)
			: base(commandName)
		{
			if (args != null && args.Length > 0)
			{
				foreach (var arg in args)
				{
					Add(arg.Key, arg.Value);
				}
			}
		}
	}
}