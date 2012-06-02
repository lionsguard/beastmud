namespace Beast.Net
{
	/// <summary>
	/// Represents a message or response from the game world.
	/// </summary>
	public interface IMessage
	{
		/// <summary>
		/// Gets or sets the identifier for the current message. Can be unique or reflect the IInput.Id.
		/// </summary>
		string Id { get; set; }
	}
}