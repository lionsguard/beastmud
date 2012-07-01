
using System.Globalization;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Beast
{
	public static class Serializer
	{
		#region JSON
		public static string ToJson<T>(this T obj)
		{
			return obj.ToJson(new JsonSerializerSettings());
		}

		public static string ToJson<T>(this T obj, JsonSerializerSettings settings)
		{
			return JsonSerialize(obj, settings);
		}

		public static T FromJson<T>(this string json)
		{
			return json.FromJson<T>(new JsonSerializerSettings());
		}

		public static T FromJson<T>(this string json, JsonSerializerSettings settings)
		{
			return JsonDeserialize<T>(json, settings);
		}

		private static JsonSerializer CreateSerializer(JsonSerializerSettings settings)
		{
			var serializer = JsonSerializer.Create(settings);
			//serializer.Converters.Add(new GameObjectJsonConverter());
			return serializer;
		}

		private static string JsonSerialize<T>(T obj, JsonSerializerSettings settings)
		{
			var serializer = CreateSerializer(settings);
			using (var sw = new StringWriter(new StringBuilder(), CultureInfo.InvariantCulture))
			{
				using (var jw = new JsonTextWriter(sw))
				{
					serializer.Serialize(jw, obj);
				}
				return sw.ToString();
			}
		}

		private static T JsonDeserialize<T>(string json, JsonSerializerSettings settings)
		{
			var serializer = CreateSerializer(settings);
			using (var sr = new StringReader(json))
			{
				using (var jr = new JsonTextReader(sr))
				{
					return serializer.Deserialize<T>(jr);
				}
			}
		}
		#endregion
	}
}
