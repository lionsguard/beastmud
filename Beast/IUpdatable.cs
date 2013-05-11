namespace Beast
{
    /// <summary>
    /// Defines and object that participates in the global update loop.
    /// </summary>
	public interface IUpdatable
	{
        /// <summary>
        /// Updates the current object.
        /// </summary>
        /// <param name="time">The current application time.</param>
		void Update(ApplicationTime time);
	}
}