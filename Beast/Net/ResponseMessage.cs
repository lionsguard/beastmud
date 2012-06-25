using System;

namespace Beast.Net
{
	/// <summary>
	/// Represents a response to player input.
	/// </summary>
	public class ResponseMessage : Message
	{
		public override MessageType Type
		{
			get { return MessageType.Command; }
		}

		public string Command { get; set; }
		public bool Success { get; set; }
		public string Error { get; set; }
		public object Data { get; set; }

		public ResponseMessage(string command)
			: this(Guid.NewGuid().ToString(), command)
		{
		}

		public ResponseMessage(string id, string command)
			: base(id)
		{
			Command = command;
			Success = true;
		}

		public ResponseMessage(IInput input)
			: this(input.Id, input.CommandName)
		{
			
		}

		public ResponseMessage Invalidate(string error)
		{
			Success = false;
			Error = error;
			return this;
		}

		public ResponseMessage Invalidate(string format, params object[] args)
		{
			return Invalidate(string.Format(format, args));
		}
	}
}