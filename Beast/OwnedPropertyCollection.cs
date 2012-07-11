using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Beast
{
	public abstract class OwnedPropertyCollection<T> : IEnumerable<KeyValuePair<string,T>>
	{
		protected IDataObject Owner { get; private set; }
		protected string Prefix { get; private set; }

		protected OwnedPropertyCollection(IDataObject owner, string prefix)
		{
			Owner = owner;
			Prefix = prefix;
		}

		protected string GetPrefixKey(string key)
		{
			if (key.StartsWith(Prefix))
				key = key.Replace(Prefix, string.Empty);
			return string.Concat(Prefix, key);
		}

		protected string GetKey(string prefixedKey)
		{
			return prefixedKey.Replace(Prefix, string.Empty);
		}

		public bool ContainsKey(string key)
		{
			return Owner.ContainsKey(GetPrefixKey(key));
		}

		public T Get(string key)
		{
			object value;
			if (!Owner.TryGetValue(GetPrefixKey(key), out value))
				return default(T);
			return (T) value;
		}

		public void Set(string key, T value)
		{
			Owner[GetPrefixKey(key)] = value;
		}

		public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
		{
			return Owner.Keys.Where(k => k.StartsWith(Prefix)).ToDictionary(GetKey, k => Owner.Get<T>(k)).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}