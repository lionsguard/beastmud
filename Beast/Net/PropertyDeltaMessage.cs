using System;
using System.Linq.Expressions;

namespace Beast.Net
{
	public class PropertyDeltaMessage : DeltaMessage
	{
		#region Overrides of Message

		public override MessageType Type
		{
			get { return MessageType.Property; }
		}

		#endregion

		/// <summary>
		/// Gets or sets the id of the containing object.
		/// </summary>
		public string ObjectId { get; set; }

		/// <summary>
		/// Gets or sets the name of the property.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the value of the property.
		/// </summary>
		public object Value { get; set; }

		/// <summary>
		/// Creates a new property delta instance.
		/// </summary>
		/// <typeparam name="TObject">The IGameObject type.</typeparam>
		/// <typeparam name="TProperty">The property which will be represented by the delta.</typeparam>
		/// <param name="obj">The IGameObject instance.</param>
		/// <param name="expression">The Expression used to resolve the property.</param>
		/// <returns>A new PropertyDeltaMessage instance.</returns>
		public static PropertyDeltaMessage Create<TObject, TProperty>(TObject obj, Expression<Func<TObject, TProperty>> expression) where TObject : IGameObject
		{
			var p = ReflectionExtensions.GetProperty(expression);
			return new PropertyDeltaMessage
			       	{
			       		ObjectId = obj.Id,
			       		Name = p.Name,
			       		Value = p.GetValue(obj)
			       	};
		}

		/// <summary>
		/// Creates a new property delta instance.
		/// </summary>
		/// <typeparam name="TObject">The IGameObject type.</typeparam>
		/// <param name="obj">The IGameObject instance.</param>
		/// <param name="propertyName">The name of the property which will be represented by the delta.</param>
		/// <returns>A new PropertyDeltaMessage instance.</returns>
		public static PropertyDeltaMessage Create<TObject>(TObject obj, string propertyName) where TObject : IGameObject
		{
			return new PropertyDeltaMessage
			       	{
			       		ObjectId = obj.Id,
			       		Name = propertyName,
			       		Value = obj.GetType().GetProperty(propertyName).GetValue(obj)
			       	};
		}
	}

}