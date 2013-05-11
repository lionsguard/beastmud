
using System;
using System.Diagnostics;

namespace Beast
{
	public static class ValueConverter
	{
		public static T Convert<T>(object value)
		{
			try
			{
				if (value == null)
					return default(T);

				var type = typeof(T);

				if (type.IsInstanceOfType(value))
					return (T)value;

				return (T)System.Convert.ChangeType(value, type);
			}
			catch (Exception ex)
			{
				Trace.TraceError(ex.ToString());
				return default(T);
			}
		}
	}
}
