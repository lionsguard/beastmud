
namespace Beast
{
	/// <summary>
	/// Represents an interactive game object; an object used explicitly within the game world and updated by the game engine.
	/// </summary>
	public interface IGameObject : IDataObject, IUpdatable
	{
		/// <summary>
		/// Gets or sets the name of the current object.
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Gets or sets the description of the current object.
		/// </summary>
		string Description { get; set; }

		/// <summary>
		/// Gets a collection of flags (Boolean properties) for the current object.
		/// </summary>
		FlagCollection Flags { get; }

		/// <summary>
		/// Gets a short descriptive string used to display the object in a message context.
		/// </summary>
		/// <returns>A string representing a short description of the object.</returns>
		string ToShortString();

		/// <summary>
		/// Gets a long descriptive string used to display the object in a verbose message context.
		/// </summary>
		/// <returns>A string representing a long or full description of the object.</returns>
		string ToLongString();
	}
}