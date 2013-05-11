using Beast.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast
{
    public class ConnectionEventArgs : EventArgs
    {
        public IConnection Connection { get; set; }

        public ConnectionEventArgs(IConnection connection)
        {
            Connection = connection;
        }
    }

    public class ProcessInputEventArgs : ConnectionEventArgs
    {
        public IInput Input { get; set; }

        public ProcessInputEventArgs(IConnection connection, IInput input)
            : base(connection)
        {
            Input = input;
        }
    }
}
