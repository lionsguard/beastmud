using System;
using Newtonsoft.Json;

namespace Beast
{
	public abstract class DataObjectJsonConverter<T> : JsonConverter where T : class, IDataObject, new()
	{

		#region Overrides of JsonConverter

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var obj = value as T;
			if (obj == null)
				return;

			writer.WriteStartObject();
			foreach (var kvp in obj)
			{
				writer.WritePropertyName(kvp.Key);
				serializer.Serialize(writer, kvp.Value);
			}
			// Write any additional properties, etc.
			WriteJsonOverride(writer, obj, serializer);
			writer.WriteEndObject();
		}

		protected virtual void WriteJsonOverride(JsonWriter writer, T value, JsonSerializer serializer)
		{
			
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var createType = objectType == typeof(T) ? typeof(T) : objectType;
			var obj = Activator.CreateInstance(createType) as T;
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
				ReadJsonOverride(reader, obj, serializer);
			}

			return obj;
		}

		protected virtual void ReadJsonOverride(JsonReader reader, T obj, JsonSerializer serializer)
		{
			
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(T) || objectType.IsSubclassOf(typeof(T)) || objectType.GetInterface(typeof(T).Name) != null;
		}

		#endregion
	}
}