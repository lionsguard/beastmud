using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beast.IO;

namespace Beast.Net.Http
{
    public class HttpConnection : ConnectionBase
    {
        private readonly Queue<IOutput> _outputQueue = new Queue<IOutput>();
        private readonly object _syncLock = new object();

        public override void Write(IOutput output)
        {
            lock (_syncLock)
            {
                _outputQueue.Enqueue(output);
            }
        }

        public override void Close()
        {
            lock (_syncLock)
            {
                _outputQueue.Clear();
            }
        }

        public IEnumerable<IOutput> Read()
        {
            lock (_syncLock)
            {
                while (_outputQueue.Count > 0)
                {
                    yield return _outputQueue.Dequeue();
                }
            }
        }

        public override void Copy(IConnection source)
        {
            base.Copy(source);

            var httpConn = source as HttpConnection;
            if (httpConn == null)
                return;

            foreach (var item in httpConn.Read())
            {
                Write(item);
            }
        }
    }
}
