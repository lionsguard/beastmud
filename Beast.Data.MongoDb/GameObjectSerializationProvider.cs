using System;
using MongoDB.Bson.Serialization;

namespace Beast.Data
{
	public class GameObjectSerializationProvider : IBsonSerializationProvider
	{
		public IBsonSerializer GetSerializer(Type type)
		{
			if (type == typeof(IGameObject) || type.IsSubclassOf(typeof(IGameObject)) || type.GetInterface(typeof(IGameObject).Name) != null)
				return new GameObjectSerializer();
			return null;
		}
	}
}