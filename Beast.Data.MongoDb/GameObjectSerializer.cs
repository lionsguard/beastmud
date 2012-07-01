using System;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace Beast.Data
{
	public class GameObjectSerializer : BsonBaseSerializer
	{
		public override object Deserialize(BsonReader bsonReader, Type nominalType, Type actualType, IBsonSerializationOptions options)
		{
			try
			{
				var doc = BsonDocument.ReadFrom(bsonReader);

				IGameObject obj = null;
				if (doc.Contains("_t"))
				{
					var objType = Game.Current.FindType(doc["_t"].AsString);
					if (objType == null)
						throw new BeastException(string.Format(Resources.DeserializeTypeNotFoundFormat, doc["_t"].AsString));
					obj = Activator.CreateInstance(objType) as IGameObject;
				}
				else
				{
					obj = Activator.CreateInstance(actualType) as IGameObject;
				}

				if (obj == null)
				{
					return BsonSerializer.Deserialize(bsonReader, nominalType, options);
				}

				foreach (var element in doc.Elements)
				{
					if (element.Value.IsObjectId)
					{
						obj.Id = element.Value.ToString();
						continue;
					}
					ReadProperty(element, obj);
				}

				return obj;

			}
			catch (Exception ex)
			{
				Log.Error(ex);
				throw;
			}
		}

		protected virtual void ReadProperty(BsonElement element, IGameObject obj)
		{
			if (element.Value.IsBsonArray)
			{
				var prop = obj.GetType().GetProperty(element.Name);
				if (prop != null)
				{
					obj[element.Name] = BsonSerializer.Deserialize(element.Value.ToJson(), prop.PropertyType);
				}
			}
			else if (element.Value.IsBsonDocument)
			{
				if (element.Value.RawValue == null)
				{
					BsonValue value;
					if (element.Value.AsBsonDocument.TryGetValue("_t", out value))
					{
						var type = Game.Current.FindType(value.AsString);
						var serializer = BsonSerializer.LookupSerializer(type);
						using (var reader = new BsonDocumentReader(element.Value.AsBsonDocument, new BsonDocumentReaderSettings()))
						{
							obj[element.Name] = serializer.Deserialize(reader, type, null);
						}
					}
					else
					{
						var propInfo = obj.GetType().GetProperty(element.Name);
						if (propInfo != null)
						{
							obj[element.Name] = BsonSerializer.Deserialize(element.Value.AsBsonDocument, propInfo.PropertyType);	
						}
					}
				}
				else
				{
					obj[element.Name] = BsonSerializer.Deserialize(element.Value.AsBsonDocument, element.Value.RawValue.GetType());
				}
			}
			else
			{
				obj[element.Name] = element.Value.RawValue;
			}
		}

		public override void Serialize(BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
		{
			try
			{
				var obj = value as IGameObject;
				if (obj == null)
				{
					BsonSerializer.Serialize(bsonWriter, nominalType, value, options);
					return;
				}

				EnsureObjectId(obj);

				bsonWriter.WriteStartDocument();
				bsonWriter.WriteString("_id", obj.Id);
				bsonWriter.WriteString("_t", obj.GetType().Name);

				foreach (var kvp in obj)
				{
					if (kvp.Key == CommonProperties.Id)
						continue;

					bsonWriter.WriteName(kvp.Key);

					if (kvp.Value != null)
					{
						var serialize = BsonSerializer.LookupSerializer(kvp.Value.GetType());
						serialize.Serialize(bsonWriter, kvp.Value.GetType(), kvp.Value, null);
					}
					else
					{
						bsonWriter.WriteNull();
					}
				}

				bsonWriter.WriteEndDocument();
			}
			catch (Exception ex)
			{
				Log.Error(ex);
			}
		}

		protected void EnsureObjectId(IGameObject obj)
		{
			if (string.IsNullOrEmpty(obj.Id))
			{
				object id;
				Type idType;
				IIdGenerator gen;
				if (GetDocumentId(obj, out id, out idType, out gen))
				{
					obj[CommonProperties.Id] = id.ToString();
				}
			}
		}

		public override bool GetDocumentId(object document, out object id, out Type idNominalType, out IIdGenerator idGenerator)
		{
			idGenerator = StringObjectIdGenerator.Instance;
			id = idGenerator.GenerateId(null, document);
			idNominalType = typeof(string);
			return true;
		}
	}
}