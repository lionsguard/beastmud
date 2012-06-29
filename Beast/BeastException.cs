using System;

namespace Beast
{
	public class BeastException : Exception
	{
		public BeastException()
		{
		}
		public BeastException(string message)
			: base(message)
		{
		}
		public BeastException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
