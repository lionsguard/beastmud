using System;
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
				writer.WritePropertyName(kvp.Key);
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

			while (reader.Read())
			{
				if (reader.TokenType == JsonToken.PropertyName)
				{
					var name = reader.ReadAsString();
					var prop = createType.GetProperty(name);
					if (prop != null)
						obj[name] = serializer.Deserialize(reader, prop.PropertyType);
					else
						obj[name] = serializer.Deserialize(reader);
				}
			}

			return obj;
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType is IGameObject || objectType.IsSubclassOf(typeof (IGameObject)) || objectType.GetInterface(typeof (IGameObject).Name) != null;
		}

		#endregion
	}
}