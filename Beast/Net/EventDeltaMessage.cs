﻿namespace Beast.Net
{
	public class EventDeltaMessage : DeltaMessage
	{
		#region Overrides of Message

		public override MessageType Type
		{
			get { return MessageType.Event; }
		}

		#endregion

		public string EventName { get; set; }
		public object Data { get; set; }

		public EventDeltaMessage(string eventName, object data)
		{
			EventName = eventName;
			Data = data;
		}
	}
}