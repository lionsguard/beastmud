using System;

namespace Beast.Net
{
	/// <summary>
	/// Represents a connected player or client.
	/// </summary>
	public interface IConnection
	{
		/// <summary>
		/// Gets or sets the unique identifier for the current connection.
		/// </summary>
		string Id { get; set; }

		/// <summary>
		/// Gets or sets the date of the last activity for the current connection.
		/// </summary>
		DateTime LastActivity { get; set; }

		/// <summary>
		/// Writes the specified IMessage instances to the current connection output stream.
		/// </summary>
		/// <param name="messages">The messages to send to the client.</param>
		void Write(params IMessage[] messages);

		/// <summary>
		/// Closes or shutdowns the current connection.
		/// </summary>
		void Close();
	}
}
