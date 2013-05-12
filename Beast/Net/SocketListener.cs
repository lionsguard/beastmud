using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace Beast.Net
{
    public class SocketListener
    {
        public Application App { get; private set; }

        private Socket _listener;

        public SocketListener(Application app)
        {
            App = app;
        }

        public void Start(IPEndPoint endPoint)
        {
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listener.Bind(endPoint);
            _listener.Listen(100);
            _listener.BeginAccept(OnAcceptCompleted, _listener);

            Trace.TraceInformation("Started SocketListener on {0}", endPoint);
        }

        public void Stop()
        {
            try
            {
                if (_listener != null)
                    _listener.Shutdown(SocketShutdown.Both);
            }
            catch (SocketException) { }
            catch (ObjectDisposedException) { }
        }

        protected virtual void OnAcceptCompleted(IAsyncResult ar)
        {
            try
            {
                // Get the listener socket.
                var listener = (Socket)ar.AsyncState;

                var socket = listener.EndAccept(ar);
                var connection = new SocketConnection(App, socket);
                App.AddConnection(connection);

                // Continue to listen for incoming connections.
                listener.BeginAccept(OnAcceptCompleted, listener);
            }
            catch (InvalidOperationException) { }
            catch (SocketException) { }
        }
    }
}
