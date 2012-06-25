using System.Collections.Generic;

namespace Beast.Net
{
	/// <summary>
	/// Represents input into the game world from a connected client.
	/// </summary>
	public interface IInput : IEnumerable<KeyValuePair<string, object>>
	{
		/// <summary>
		/// Gets or sets the unique identifier of the current input message.
		/// </summary>
		string Id { get; set; }

		/// <summary>
		/// Gets or sets the identifier of the current connection.
		/// </summary>
		string ConnectionId { get; set; }

		/// <summary>
		/// Gets or sets the name of the command to execute.
		/// </summary>
		string CommandName { get; set; }

		/// <summary>
		/// Gets the current number of key/value pairs stored with the input.
		/// </summary>
		int Count { get; }

		/// <summary>
		/// Gets the value for the specified key.
		/// </summary>
		/// <param name="key">The key used to locate the value.</param>
		/// <returns>The value of the specified key.</returns>
		object this[string key] { get; }

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
	}
}