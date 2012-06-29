using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Beast
{
	public class GameObjectJsonConverter : JsonConverter
	{
		#region Overrides of JsonConverter

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var obj = value as IGameObject;
			if (obj == null)
				return;

			writer.WriteStartObject();
			foreach (var kvp in obj)
			{
				writer.WritePropertyName(kvp.Key.Name);
				serializer.Serialize(writer, kvp.Value);
			}
			writer.WriteEndObject();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var createType = objectType == typeof (IGameObject) ? typeof (GameObject) : objectType;
			var obj = Activator.CreateInstance(createType) as IGameObject;
			if (obj == null)
				return null;

			// Serialize the reader data into a dictionary.
			var values = serializer.Deserialize<Dictionary<string, object>>(reader);

			// Find all property definitions.
			var props = Property.FindProperties(obj);
			
			// Set the values on the object.
			obj.Merge(values, props);

			return obj;
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType is IGameObject || objectType.IsSubclassOf(typeof (IGameObject)) || objectType.GetInterface(typeof (IGameObject).Name) != null;
		}

		#endregion
	}
}