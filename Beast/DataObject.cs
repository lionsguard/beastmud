using System;
using System.Collections;
using System.Collections.Generic;

namespace Beast
{
	public abstract class DataObject : IDataObject
	{
		private readonly Dictionary<string, object> _properties = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

		public object this[string key]
		{
			get { return Get(key); }
			set{Set(key, value);}
		}

		public string Id
		{
			get { return Get<string>(CommonProperties.Id); }
			set { Set(CommonProperties.Id, value); }
		}

		#region Property Handling
		public T Get<T>(string property)
		{
			var value = Get(property);
			if (value == null)
				return default(T);
			if (value.GetType() == typeof(T))
				return (T) value;
			if (value is IConvertible)
				return (T) Convert.ChangeType(value, typeof (T));
			return (T) value;
		}

		protected internal object Get(string property)
		{
			object value;
			_properties.TryGetValue(property, out value);
			return value;
		}
		protected internal void Set(string property, object value)
		{
			_properties[property] = value;
		}

		public void Merge(IDictionary<string, object> values, bool overwriteExisting)
		{
			foreach (var kvp in values)
			{
				if (!overwriteExisting && _properties.ContainsKey(kvp.Key))
					continue;

				_properties[kvp.Key] = kvp.Value;
			}
		}
		#endregion

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return _properties.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#region Explicit Members
		bool IDictionary<string, object>.ContainsKey(string key)
		{
			return _properties.ContainsKey(key);
		}

		void IDictionary<string, object>.Add(string key, object value)
		{
			Set(key, value);
		}

		bool IDictionary<string, object>.Remove(string key)
		{
			return _properties.Remove(key);
		}

		bool IDictionary<string, object>.TryGetValue(string key, out object value)
		{
			return _properties.TryGetValue(key, out value);
		}

		ICollection<string> IDictionary<string, object>.Keys
		{
			get { return _properties.Keys; }
		}

		ICollection<object> IDictionary<string, object>.Values
		{
			get { return _properties.Values; }
		}

		void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
		{
			Set(item.Key, item.Value);
		}

		void ICollection<KeyValuePair<string, object>>.Clear()
		{
			_properties.Clear();
		}

		bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
		{
			return _properties.ContainsKey(item.Key);
		}

		void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<string, object>>)_properties).CopyTo(array, arrayIndex);
		}

		bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
		{
			return _properties.Remove(item.Key);
		}

		int ICollection<KeyValuePair<string, object>>.Count
		{
			get { return _properties.Count; }
		}

		bool ICollection<KeyValuePair<string, object>>.IsReadOnly
		{
			get { return false; }
		}

		#endregion
	}
}