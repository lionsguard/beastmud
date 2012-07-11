using System.Collections.Generic;

namespace Beast
{
	/// <summary>
	/// Represents a generic data object wrapping the functionality of a string/object Dictionary.
	/// </summary>
	public interface IDataObject : IDictionary<string,object>
	{
		/// <summary>
		/// Gets or sets the unique identifier of the object.
		/// </summary>
		string Id { get; set; }

		/// <summary>
		/// Gets the type cast value of the specified property.
		/// </summary>
		/// <typeparam name="T">The System.Type of the value to return.</typeparam>
		/// <param name="property">The name of the property in which the value should be retrieved.</param>
		/// <returns>The value of the specified property.</returns>
		T Get<T>(string property);

		/// <summary>
		/// Merges the specified object with the current object, overwriting existing values if specified.
		/// </summary>
		/// <param name="values">The dictionary containing properties to set on the current object.</param>
		/// <param name="overwriteExisting">True to overwrite any existing properties, false to leave properties.</param>
		void Merge(IDictionary<string, object> values, bool overwriteExisting);
	}
}