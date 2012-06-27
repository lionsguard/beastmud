
using System.Collections.Generic;

namespace Beast
{
	/// <summary>
	/// Represents an interactive game object; an object used explicitly within the game world and updated by the game engine.
	/// </summary>
	public interface IGameObject : IUpdatable, IEnumerable<KeyValuePair<Property,object>>
	{
		/// <summary>
		/// Gets or sets the unique identifier of the object.
		/// </summary>
		string Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the current object.
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Gets or sets the description of the current object.
		/// </summary>
		string Description { get; set; }

		/// <summary>
		/// Gets or sets a property with the specified name.
		/// </summary>
		/// <param name="property">The Property metadata for the property to get or set.</param>
		/// <returns>The value of the property.</returns>
		object this[Property property] { get; set; }

		/// <summary>
		/// Gets the type cast value of the specified property.
		/// </summary>
		/// <typeparam name="T">The System.Type of the value to return.</typeparam>
		/// <param name="property">The property containing the metadata describing the attribute to retrieve.</param>
		/// <returns>The value of the specified property.</returns>
		T Get<T>(Property property);

		/// <summary>
		/// Merges the specified object with the current object, overwriting existing values if specified.
		/// </summary>
		/// <param name="obj">The object containing properties to set on the current object.</param>
		/// <param name="overwriteExisting">True to overwrite any existing properties, false to leave properties.</param>
		void Merge(IGameObject obj, bool overwriteExisting);

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