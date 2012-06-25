using System;
using System.Collections;
using System.Collections.Generic;

namespace Beast.Net
{
	public abstract class InputBase : IInput
	{
		private readonly Dictionary<string,object> _values = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

		protected InputBase()
		{
			Id = Guid.NewGuid().ToString();
		}
		protected InputBase(string commandName) 
			: this()
		{
			CommandName = commandName;
		}

		public void Add(string key, object value)
		{
			_values[key] = value;
		}

		#region Implementation of IEnumerable

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return _values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		#region Implementation of IInput

		public string Id { get; set; }
		public string ConnectionId { get; set; }
		public string CommandName { get; set; }

		public int Count
		{
			get { return _values.Count; }
		}

		public object this[string key]
		{
			get
			{
				object value;
				_values.TryGetValue(key, out value);
				return value;
			}
			set { _values[key] = value; }
		}

		public T Get<T>(string key)
		{
			object value;
			if (_values.TryGetValue(key, out value))
			{
				return ValueConverter.Convert<T>(value);
			}
			return default(T);
		}

		public bool TryGetValue<T>(string key, out T value)
		{
			object objValue;
			if (!_values.TryGetValue(key, out objValue))
			{
				value = default(T);
				return false;
			}
			value = ValueConverter.Convert<T>(objValue);
			return true;
		}

		#endregion

		public bool Contains(string key)
		{
			return _values.ContainsKey(key);
		}

		public ICollection<string> Keys
		{
			get { return _values.Keys; }
		}

		public ICollection<object> Values
		{
			get { return _values.Values; }
		}
	}
}
