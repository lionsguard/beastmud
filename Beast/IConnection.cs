using Beast.IO;

namespace Beast
{
    /// <summary>
    /// Represents a connection to the application.
    /// </summary>
    public interface IConnection
    {
        /// <summary>
        /// Gets or sets a unique identifier for the connection.
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Gets or sets the tick on which the last activity occured on this connection.
        /// </summary>
        long LastActivityTick { get; set; }

        /// <summary>
        /// Writes output to the current connection.
        /// </summary>
        /// <param name="output">The output to write to the connection.</param>
        void Write(IOutput output);

        /// <summary>
        /// Closes the connection.
        /// </summary>
        void Close();
        
        /// <summary>
        /// Gets the value for the specified key.
        /// </summary>
        /// <typeparam name="T">The System.Type of the value to retrieve.</typeparam>
        /// <param name="key">The key of the value to retrieve.</param>
        /// <param name="defaultValue">A default value if a value is not found for the key.</param>
        /// <returns>The value or default for the specified key.</returns>
        T Get<T>(string key, T defaultValue);

        /// <summary>
        /// Sets the value for the specified key.
        /// </summary>
        /// <param name="key">The key for which to set the value.</param>
        /// <param name="value">The value to set.</param>
        void Set(string key, object value);

        /// <summary>
        /// Copies the values from the source connection into the current connection. All values except Id are copied.
        /// </summary>
        /// <param name="source">The IConnection to copy from.</param>
        void Copy(IConnection source);
    }
}
