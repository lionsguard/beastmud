using System.Collections.Generic;

namespace Beast.IO
{
    /// <summary>
    /// Represents input into the BeastMUD Application, intended to execute a command or process information within a module.
    /// </summary>
    public interface IInput : IEnumerable<KeyValuePair<string, object>>
    {
        /// <summary>
        /// Gets the unique id for the current input.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the current number of key/value pairs stored with the input.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets or sets the value for the specified key.
        /// </summary>
        /// <param name="key">The key used to locate the value.</param>
        /// <returns>The value of the specified key.</returns>
        object this[string key] { get; set; }

        /// <summary>
        /// Gets a value from the input of the specified type for the specified key.
        /// </summary>
        /// <typeparam name="T">The type of the value to return.</typeparam>
        /// <param name="key">The key used to locate the value.</param>
        /// <returns>The value of the specified key cast as T.</returns>
        T Get<T>(string key);

        /// <summary>
        /// Attempts to retrieve the specified value suing the specified key.
        /// </summary>
        /// <typeparam name="T">The type of the value to return.</typeparam>
        /// <param name="key">The key used to locate the value.</param>
        /// <param name="value">The variable to hold the returned value.</param>
        /// <returns>True if a value was found for the specified key; otherwise false.</returns>
        bool TryGetValue<T>(string key, out T value);

        /// <summary>
        /// Gets a value indicating whether or not the current input contains a value with the specified key.
        /// </summary>
        /// <param name="key">The key to find.</param>
        /// <returns>True if an entry exists with the specified key; otherwise false.</returns>
        bool Contains(string key);

        /// <summary>
        /// Creates an IOutput instance intended to be the response to the current input.
        /// </summary>
        /// <returns>An IOutput instance representing the response to the current input.</returns>
        IOutput CreateOutput();

        /// <summary>
        /// Loads the input from the specified byte array.
        /// </summary>
        /// <param name="data">The byte array containing the key/value pairs to load.</param>
        void FromBytes(byte[] data);

        /// <summary>
        /// Loads the input from the specified string.
        /// </summary>
        /// <param name="data">The string containing the key/value pairs to load.</param>
        void FromString(string data);

        /// <summary>
        /// Loads the input from the specified JSON string.
        /// </summary>
        /// <param name="data">The JSON string containing the key/value pairs to load.</param>
        void FromJson(string json);
    }
}
