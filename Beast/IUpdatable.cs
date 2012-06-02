namespace Beast
{
	/// <summary>
	/// Represents an object that can be updated and sync with the specified game time.
	/// </summary>
	public interface IUpdatable
	{
		/// <summary>
		/// Updates the current object.
		/// </summary>
		/// <param name="gameTime">The current GameTime of the running game.</param>
		void Update(GameTime gameTime);
	}
}