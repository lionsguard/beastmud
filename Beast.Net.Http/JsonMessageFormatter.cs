
namespace Beast.Net
{
	public class JsonMessageFormatter : IMessageFormatter<string>
	{
		#region Implementation of IMessageFormatter<string>

		public string FormatMessage(IMessage message)
		{
			return message.ToJson();
		}

		public IInput FormatInput(string data)
		{
			return data.FromJson<IInput>();
		}

		#endregion
	}
}