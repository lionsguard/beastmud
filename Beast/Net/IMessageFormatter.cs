using Beast.Commands;

namespace Beast.Net
{
	/// <summary>
	/// Represents a message formatter used to convert messages and input to and from other types.
	/// </summary>
	/// <typeparam name="T">The type in which to convert to and from.</typeparam>
	public interface IMessageFormatter<T>
	{
		/// <summary>
		/// Converts the specified IMessage instance to the specified type.
		/// </summary>
		/// <param name="message">The message to convert.</param>
		/// <returns>The message formatted into the specified type.</returns>
		T FormatMessage(IMessage message);

		/// <summary>
		/// Formats the specified data into a Command instance.
		/// </summary>
		/// <param name="data">The data to format.</param>
		/// <returns>An Command instance for the specified data.</returns>
		Command FormatCommand(T data);
	}
}