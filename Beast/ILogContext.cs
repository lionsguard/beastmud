namespace Beast
{
    /// <summary>
    /// Defines an object that can handle logging messages.
    /// </summary>
	public interface ILogContext
	{
        /// <summary>
        /// Writes a message to the current logging contexts.
        /// </summary>
        /// <param name="type">The LogType of the message to write.</param>
        /// <param name="message">The message to write.</param>
		void Write(LogType type, string message);
	}
}