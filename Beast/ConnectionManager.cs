using Beast.IO;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast
{
    internal class ConnectionManager : IUpdatable, IDisposable
    {
        private readonly ConcurrentDictionary<string, IConnection> _connections = new ConcurrentDictionary<string, IConnection>(StringComparer.InvariantCultureIgnoreCase);
        private readonly TimeSpan _timeout;

        public event EventHandler<ConnectionEventArgs> ConnectionAdded = delegate { };
        public event EventHandler<ConnectionEventArgs> ConnectionRemoved = delegate { };
        
        public int Count
        {
            get { return _connections.Count; }
        }

        public ConnectionManager(TimeSpan timeout)
        {
            _timeout = timeout;
        }

        public void Add(IConnection connection)
        {
            _connections.AddOrUpdate(connection.Id, connection, (key, existing) => connection);
            ConnectionAdded(this, new ConnectionEventArgs(connection));
        }

        public void Remove(string connectionId)
        {
            IConnection removed;
            if (_connections.TryRemove(connectionId, out removed))
            {
                removed.Close();
                ConnectionRemoved(this, new ConnectionEventArgs(removed));
            }
        }

        public IConnection Get(string id)
        {
            IConnection conn;
            _connections.TryGetValue(id, out conn);
            return conn;
        }

        public IEnumerable<IConnection> Find(Func<IConnection, bool> predicate)
        {
            return _connections.Values.Where(predicate);
        }

        public void Broadcast(IOutput output)
        {
            foreach (var conn in _connections.Values)
            {
                conn.Write(output);
            }
        }

        public void Update(ApplicationTime time)
        {
            var connections = _connections.Values.Where(c => (time.Ticks - c.LastActivityTick) > _timeout.Ticks).ToArray();
            Task.Run(() =>
                {
                    foreach (var conn in connections)
                    {
                        Remove(conn.Id);
                    }
                });
        }

        public void Dispose()
        {
            _connections.Values.ToList().ForEach(c => c.Close());
            _connections.Clear();
        }
    }
}
