using Beast.IO;
using System;
using System.Collections.Generic;

namespace Beast
{
    /// <summary>
    /// Provides an abstract implementation of the IConnection interface
    /// </summary>
    public abstract class ConnectionBase : IConnection
    {
        /// <summary>
        /// Gets the dictionary used to store key/value pairs of data on the connection.
        /// </summary>
        protected Dictionary<string, object> InnerDictionary { get; private set; }

        /// <summary>
        /// Gets or sets a unique identifier for the connection.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the tick on which the last activity occured on this connection.
        /// </summary>
        public long LastActivityTick { get; set; }

        /// <summary>
        /// Initializes a new instance of the ConnectionBase class.
        /// </summary>
        protected ConnectionBase()
        {
            Id = Guid.NewGuid().ToString();
            InnerDictionary = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Writes output to the current connection.
        /// </summary>
        /// <param name="output">The output to write to the connection.</param>
        public abstract void Write(IOutput output);

        /// <summary>
        /// Closes the connection.
        /// </summary>
        public virtual void Close()
        {
        }

        /// <summary>
        /// Gets the value for the specified key.
        /// </summary>
        /// <typeparam name="T">The System.Type of the value to retrieve.</typeparam>
        /// <param name="key">The key of the value to retrieve.</param>
        /// <param name="defaultValue">A default value if a value is not found for the key.</param>
        /// <returns>The value or default for the specified key.</returns>
        public T Get<T>(string key, T defaultValue)
        {
            object value;
            if (!InnerDictionary.TryGetValue(key, out value))
                return defaultValue;
            return ValueConverter.Convert<T>(value);
        }

        /// <summary>
        /// Sets the value for the specified key.
        /// </summary>
        /// <param name="key">The key for which to set the value.</param>
        /// <param name="value">The value to set.</param>
        public void Set(string key, object value)
        {
            InnerDictionary[key] = value;
        }
    }
}
