using System.Collections.Generic;
using System.Collections.Specialized;

namespace Beast.Net
{
	public class JsonInput : InputBase, IDictionary<string,object>
	{
		#region Implementation of ICollection<KeyValuePair<string,object>>

		public void Add(KeyValuePair<string, object> item)
		{
			Add(item.Key, item.Value);
		}

		public void Clear()
		{
		}

		public bool Contains(KeyValuePair<string, object> item)
		{
			return ContainsKey(item.Key);
		}

		public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			var index = arrayIndex;
			foreach (var kvp in this)
			{
				array[index] = kvp;
				index++;
			}
		}

		public bool Remove(KeyValuePair<string, object> item)
		{
			return true;
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		#endregion

		#region Implementation of IDictionary<string,object>

		public bool ContainsKey(string key)
		{
			return Contains(key);
		}

		public bool Remove(string key)
		{
			return true;
		}

		public bool TryGetValue(string key, out object value)
		{
			return base.TryGetValue(key, out value);
		}

		#endregion

		public static JsonInput FromRequestParams(NameValueCollection parameters)
		{
			var input = new JsonInput();
			foreach (var key in parameters.AllKeys)
			{
				input.Add(key, parameters[key]);
			}
			return input;
		}
	}
}