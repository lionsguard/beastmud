using System;
using System.Collections.Concurrent;
using System.Linq;
using Beast.Commands;

namespace Beast.Net
{
	/// <summary>
	/// Manages connected clients.
	/// </summary>
	public static class ConnectionManager
	{
		private static readonly ConcurrentDictionary<string,IConnection> Connections = new ConcurrentDictionary<string, IConnection>(StringComparer.InvariantCultureIgnoreCase);

		/// <summary>
		/// Gets the timeout value used to close and removed stale clients.
		/// </summary>
		public static TimeSpan Timeout { get; private set; }

		static ConnectionManager()
		{
			Timeout = Game.DefaultConnectionTimeout;
		}

		/// <summary>
		/// Initializes the ConnectionManager.
		/// </summary>
		/// <param name="timeout">The timeout value used to disconnect stale clients.</param>
		internal static void Initialize(TimeSpan timeout)
		{
			Timeout = timeout;
		}

		/// <summary>
		/// Finds an IConnection instance for the specified connectionId.
		/// </summary>
		/// <param name="connectionId">The id of the connection to find.</param>
		/// <returns>An IConnection instance if found; otherwise null.</returns>
		public static IConnection Find(string connectionId)
		{
			IConnection connection;
			Connections.TryGetValue(connectionId, out connection);
			return connection;
		}

		/// <summary>
		/// Replaces the current IConnection instance with the specified instance.
		/// </summary>
		/// <param name="connection">The IConnection instance that will replace the current.</param>
		public static void Replace(IConnection connection)
		{
			Connections[connection.Id] = connection;
		}

		/// <summary>
		/// Creates an IConnection instance and begins tracking it.
		/// </summary>
		/// <param name="factory">The IConnectionFactory instance used to create the actual IConnection instance.</param>
		/// <returns>The newly created IConnection instance.</returns>
		public static IConnection Create(IConnectionFactory factory)
		{
			var conn = factory.CreateConnection();
			Connections.TryAdd(conn.Id, conn);
			return conn;
		}

		private static void CleanupConnections()
		{
			var now = DateTime.UtcNow;
			var removes = (from connection in Connections.Values where (now - connection.LastActivity) > Timeout select connection.Id);

			foreach (var remove in removes)
			{
				IConnection conn;
				Connections.TryRemove(remove, out conn);
			}
		}

		/// <summary>
		/// Checks and processes input from connected clients.
		/// </summary>
		public static void CheckInput()
		{
			CleanupConnections();

			var connections = Connections.Values.ToArray();
			foreach (var connection in connections)
			{
				// Retrieve and process all queued commands on the connection, clearing them out as they are processed.
				var commands = connection.DequeueCommands();
				foreach (var command in commands)
				{
					CommandManager.Execute(command, connection);
				}
			}
		}

		/// <summary>
		/// Flushes all output of all connected clients.
		/// </summary>
		public static void Flush()
		{
			var connections = Connections.Values.ToArray();
			foreach (var connection in connections)
			{
				// If the connection has a character instance, read the deltas and write them to the connection.
				if (connection.Character != null)
				{
					connection.Write(connection.Character.DequeueMessages().ToArray());
				}

				// Flush the current connection, causing all queued messages to be written.
				connection.Flush();
			}
		}
	}
}