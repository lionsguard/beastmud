using System;
using System.Net;
using System.Net.Sockets;

namespace Beast.Net
{
	public class SocketListener
	{
		private Socket _listener;
		private SocketConnectionFactory _factory;
		private Action<IConnection> _onNewConnection;

		public void Start(IPEndPoint endPoint, IMessageFormatter<byte[]> messageFormatter, Action<IConnection> onNewConnection)
		{
			_onNewConnection = onNewConnection;
			_factory = new SocketConnectionFactory(messageFormatter);

			_listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			_listener.Bind(endPoint);
			_listener.Listen(100);
			_listener.BeginAccept(OnAcceptCompleted, _listener);

			Log.Info("Socket Listener started on {0}", endPoint);
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

		private void OnAcceptCompleted(IAsyncResult ar)
		{
			try
			{
				// Get the listener socket.
				var listener = (Socket)ar.AsyncState;

				// Initialize the Connection class for handling client connections.
				var socket = listener.EndAccept(ar);
				var connection = ConnectionManager.Create(_factory) as SocketConnection;
				if (connection != null)
				{
					connection.Manage(socket);
					Log.Debug("Connection received from {0}", socket.RemoteEndPoint);

					if (_onNewConnection != null)
						_onNewConnection(connection);
				}

				// Continue to listen for incoming connections.
				listener.BeginAccept(OnAcceptCompleted, listener);
			}
			catch (InvalidOperationException) { }
			catch (SocketException) { }
		}
	}
}
