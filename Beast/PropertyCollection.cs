using System;
using System.Collections.Generic;

namespace Beast
{
	public class PropertyCollection : Dictionary<string, object>
	{
		public PropertyCollection()
			: base(StringComparer.InvariantCultureIgnoreCase)
		{
			
		}

		public T ValueOrDefault<T>(string key)
		{
			object value;
			if (TryGetValue(key, out value))
			{
				if (value != null)
					return (T) value;
			}
			return default(T);
		}
	}
}