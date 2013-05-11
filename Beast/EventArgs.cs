using Beast.Commands;
using Beast.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast
{
	public class ApplicationErrorEventArgs : EventArgs
	{
		public Exception Error {get;set;}
		
		public ApplicationErrorEventArgs(Exception error)
		{
			Error = error;
		}
	}

    public class ConnectionEventArgs : EventArgs
    {
        public IConnection Connection { get; set; }

        public ConnectionEventArgs(IConnection connection)
        {
            Connection = connection;
        }
    }

    public class InputEventArgs : ConnectionEventArgs
    {
        public IInput Input { get; set; }

        public InputEventArgs(IConnection connection, IInput input)
            : base(connection)
        {
            Input = input;
        }
    }

    public class CommandNotFoundEventArgs : InputEventArgs
    {
        public string CommandName { get; set; }

        public CommandNotFoundEventArgs(string commandName, IConnection connection, IInput input)
            : base(connection, input)
        {
            CommandName = commandName;
        }
    }
}
