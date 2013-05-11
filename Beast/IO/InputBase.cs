using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Beast.IO
{
    /// <summary>
    /// Provides an abstract implementation of the IInput interface.
    /// </summary>
    public abstract class InputBase : IInput
    {
		private readonly Dictionary<string, object> _values = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// Initializes a new instance of the InputBase class.
        /// </summary>
        /// <param name="values">An array of values for the input.</param>
		protected InputBase(params KeyValuePair<string, object>[] values)
		{
			Id = Guid.NewGuid().ToString();

			if (values != null && values.Length > 0)
			{
				foreach (var value in values)
				{
					if (string.Equals(value.Key, "Id", StringComparison.InvariantCultureIgnoreCase))
					{
						Id = (string)value.Value;
						continue;
					}

					_values.Add(value.Key, value.Value);
				}
			}
		}

        /// <summary>
        /// Initializes a new instance of the InputBase class.
        /// </summary>
        /// <param name="values">An array of values for the input.</param>
        protected InputBase(IEnumerable<KeyValuePair<string, object>> values)
			: this(values.ToArray())
		{
		}

        /// <summary>
        /// Initializes a new instance of the InputBase class.
        /// </summary>
        /// <param name="values">An array of values for the input.</param>
        protected InputBase(NameValueCollection collection)
			: this(collection.AllKeys.ToDictionary(k => k, k => (object)collection[k]).ToArray())
		{
		}

        /// <summary>
        /// Adds a new entry into the input collection.
        /// </summary>
        /// <param name="key">The key of the entry to add.</param>
        /// <param name="value">The vlaue of the entry to add.</param>
		public void Add(string key, object value)
		{
			_values[key] = value;
		}

		#region Implementation of IEnumerable

        /// <summary>
        /// Gets an IEnumerator for the current input.
        /// </summary>
        /// <returns>An IEnumerator for the current input.</returns>
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

        /// <summary>
        /// Gets the unique id for the current input.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the current number of key/value pairs stored with the input.
        /// </summary>
        public int Count
		{
			get { return _values.Count; }
		}

        /// <summary>
        /// Gets or sets the value for the specified key.
        /// </summary>
        /// <param name="key">The key used to locate the value.</param>
        /// <returns>The value of the specified key.</returns>
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

        /// <summary>
        /// Gets a value from the input of the specified type for the specified key.
        /// </summary>
        /// <typeparam name="T">The type of the value to return.</typeparam>
        /// <param name="key">The key used to locate the value.</param>
        /// <returns>The value of the specified key cast as T.</returns>
        public T Get<T>(string key)
		{
			object value;
			if (_values.TryGetValue(key, out value))
			{
				return ValueConverter.Convert<T>(value);
			}
			return default(T);
		}

        /// <summary>
        /// Attempts to retrieve the specified value suing the specified key.
        /// </summary>
        /// <typeparam name="T">The type of the value to return.</typeparam>
        /// <param name="key">The key used to locate the value.</param>
        /// <param name="value">The variable to hold the returned value.</param>
        /// <returns>True if a value was found for the specified key; otherwise false.</returns>
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

        /// <summary>
        /// Gets a value indicating whether or not the current input contains a value with the specified key.
        /// </summary>
        /// <param name="key">The key to find.</param>
        /// <returns>True if an entry exists with the specified key; otherwise false.</returns>
        public bool Contains(string key)
		{
			return _values.ContainsKey(key);
		}

        /// <summary>
        /// Gets a collection of keys contained in the current instance.
        /// </summary>
		public ICollection<string> Keys
		{
			get { return _values.Keys; }
		}

        /// <summary>
        /// Gets a collection of values contained in the current instance.
        /// </summary>
		public ICollection<object> Values
		{
			get { return _values.Values; }
		}

        /// <summary>
        /// Creates an IOutput instance intended to be the response to the current input.
        /// </summary>
        /// <returns>An IOutput instance representing the response to the current input.</returns>
        public abstract IOutput CreateOutput();
    }
}
