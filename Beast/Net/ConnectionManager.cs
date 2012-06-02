using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Beast.Net
{
	public static class ConnectionManager
	{
		private static readonly ConcurrentDictionary<string,IConnection> Connections = new ConcurrentDictionary<string, IConnection>(StringComparer.InvariantCultureIgnoreCase);

		public static TimeSpan Timeout { get; private set; }

		static ConnectionManager()
		{
			Timeout = GameSettings.DefaultConnectionTimeout;
		}

		public static void Initialize(TimeSpan timeout)
		{
			Timeout = timeout;
		}

		public static IConnection Find(string connectionId)
		{
			IConnection connection;
			Connections.TryGetValue(connectionId, out connection);
			return connection;
		}

		public static void Replace(IConnection connection)
		{
			Connections[connection.Id] = connection;
		}

		public static IConnection Create(IConnectionFactory factory)
		{
			var conn = factory.CreateConnection();
			Connections.TryAdd(conn.Id, conn);
			return conn;
		}

		public static void Update(GameTime gameTime)
		{
			var now = DateTime.UtcNow;
			var removes = (from connection in Connections.Values where (now - connection.LastActivity) > Timeout select connection.Id);

			foreach (var remove in removes)
			{
				IConnection conn;
				Connections.TryRemove(remove, out conn);
			}
		}
	}
}