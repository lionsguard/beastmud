namespace Beast
{
    /// <summary>
    /// Defines an object that can be created and shutdown and will participate in server start and stop events.
    /// </summary>
	public interface IInitializable
	{
        /// <summary>
        /// Initializes the object.
        /// </summary>
		void Initialize();

        /// <summary>
        /// Causes the object to clean up resources and shutdown.
        /// </summary>
		void Shutdown();
	}
}