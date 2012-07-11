using System;
using Beast.Net;

namespace Beast
{
	/// <summary>
	/// Represents an object used to create instances of IGameObjects using IInput.
	/// </summary>
	public interface IGameObjectCreator
	{
		/// <summary>
		/// Determines whether or not the current instance can create the specified type.
		/// </summary>
		/// <param name="type">The type of object to create.</param>
		/// <returns>True if this instance can create the specified type; otherwise false.</returns>
		bool CanCreate(Type type);

		/// <summary>
		/// Attempts to create an instance of the specified type using the specified input.
		/// </summary>
		/// <param name="type">The type of object to create.</param>
		/// <param name="input">The IInput instance containing the input that defines the properties of the object to create.</param>
		/// <param name="errorMessage">An error message that will be set if the creation of the type fails.</param>
		/// <param name="obj">The instance created upon a successful operation.</param>
		/// <returns>True if the object was created; otherwise false.</returns>
		bool TryCreate(Type type, IInput input, out string errorMessage, out IGameObject obj);
	}
}
