
using System;
using System.Collections.Generic;
using System.Linq;
using Beast.Mobiles;
using Beast.Security;

namespace Beast.Net
{
	public class MvcConnection : IConnection
	{
		private readonly IConnection _connection;
		private IInput _currentInput;
		private ResponseMessage _currentResponse;

		private MvcConnection(IConnection connection)
		{
			_connection = connection;
		}

		public ResponseMessage ProcessInput(IInput input)
		{
			// Record the current input in order to match the response during the Write operation.
			_currentInput = input;

			// Execute the command immediately.
			Game.Current.Commands.Execute(input, this);

			// Snag the current response and then clear the current input and response for the next operation.
			var response = _currentResponse;
			_currentInput = null;
			_currentResponse = null;
			return response;
		}

		public static MvcConnection Wrap(IConnection connection)
		{
			return new MvcConnection(connection);
		}

		#region Implementation of IConnection

		public string Id
		{
			get { return _connection.Id; }
			set { _connection.Id = value; }
		}

		public DateTime LastActivity
		{
			get { return _connection.LastActivity; }
			set { _connection.LastActivity = value; }
		}

		public User User
		{
			get { return _connection.User; }
			set { _connection.User = value; }
		}

		public Character Character
		{
			get { return _connection.Character; }
			set { _connection.Character = value; }
		}

		public void EnqueueInput(IInput input)
		{
			_connection.EnqueueInput(input);
		}

		public IEnumerable<IInput> DequeueInput()
		{
			return _connection.DequeueInput();
		}

		public void Write(params IMessage[] messages)
		{
			if (_currentResponse == null && _currentInput != null)
				_currentResponse = messages.Where(m => m is ResponseMessage).Select(m => (ResponseMessage)m).FirstOrDefault(m => m.Command == _currentInput.CommandName);
			_connection.Write(messages);
		}

		public void Flush()
		{
			_connection.Flush();
		}

		public void Close()
		{
			_connection.Close();
		}

		#endregion
	}
}
