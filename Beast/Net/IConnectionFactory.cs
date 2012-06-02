namespace Beast.Net
{
	/// <summary>
	/// Represents an object used to create IConnection instances.
	/// </summary>
	public interface IConnectionFactory
	{
		/// <summary>
		/// Creates a new IConnection instance.
		/// </summary>
		/// <returns>A new IConnection instance.</returns>
		IConnection CreateConnection();
	}
}