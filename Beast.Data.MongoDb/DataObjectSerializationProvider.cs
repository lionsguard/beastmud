using System;
using MongoDB.Bson.Serialization;

namespace Beast.Data
{
	public class DataObjectSerializationProvider : IBsonSerializationProvider
	{
		public IBsonSerializer GetSerializer(Type type)
		{
			if (type == typeof(IGameObject) || type.IsSubclassOf(typeof(IGameObject)) || type.GetInterface(typeof(IGameObject).Name) != null)
				return new GameObjectSerializer();
			if (type == typeof(IDataObject) || type.IsSubclassOf(typeof(IDataObject)) || type.GetInterface(typeof(IDataObject).Name) != null)
				return new DataObjectSerializer<IDataObject>();
			return null;
		}
	}
}