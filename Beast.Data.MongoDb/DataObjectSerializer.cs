using System;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace Beast.Data
{
	public class DataObjectSerializer<T> : BsonBaseSerializer, IBsonIdProvider where T : IDataObject
	{
		public override object Deserialize(BsonReader bsonReader, Type nominalType, Type actualType, IBsonSerializationOptions options)
		{
			try
			{
				var doc = BsonDocument.ReadFrom(bsonReader);

				object instance = null;
				if (doc.Contains("_t"))
				{
					var objType = Game.Current.FindType(doc["_t"].AsString);
					if (objType == null)
						throw new BeastException(string.Format(Resources.DeserializeTypeNotFoundFormat, doc["_t"].AsString));
					instance = Activator.CreateInstance(objType);
				}
				else
				{
					instance = Activator.CreateInstance(actualType);
				}

				if (!(instance is T))
				{
					return BsonSerializer.Deserialize(bsonReader, nominalType, options);
				}
				var obj = (T)instance;

				foreach (var element in doc.Elements)
				{
					if (element.Name == "_id")
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

		protected virtual void ReadProperty(BsonElement element, T obj)
		{
			if (element.Value.IsBsonArray)
			{
				var prop = obj.GetType().GetProperty(element.Name);
				if (prop != null)
				{
					obj[element.Name] = BsonSerializer.Deserialize(BsonExtensionMethods.ToJson(element.Value), prop.PropertyType);
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
				if (!(value is T))
				{
					BsonSerializer.Serialize(bsonWriter, nominalType, value, options);
					return;
				}

				var obj = (T)value;
				EnsureObjectId(obj);

				bsonWriter.WriteStartDocument();
				bsonWriter.WriteString("_id", obj.Id);
				bsonWriter.WriteString("_t", obj.GetType().Name);

				foreach (var kvp in obj)
				{
					if (kvp.Key == CommonProperties.Id)
						continue;

					if (kvp.Key == "_t")
						continue;

					bsonWriter.WriteName(kvp.Key);

					if (kvp.Value != null)
					{
						var serializer = BsonSerializer.LookupSerializer(kvp.Value.GetType());
						serializer.Serialize(bsonWriter, kvp.Value.GetType(), kvp.Value, null);
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

		protected virtual void EnsureObjectId(T obj)
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

		public bool GetDocumentId(object document, out object id, out Type idNominalType, out IIdGenerator idGenerator)
		{
			id = null;
			if (document is IDataObject)
			{
				id = (document as IDataObject).Id;
			}

			idGenerator = StringObjectIdGenerator.Instance;
			if (id == null)
				id = idGenerator.GenerateId(null, document);
			idNominalType = typeof(string);
			return true;
		}

		public void SetDocumentId(object document, object id)
		{
			if (!(document is IDataObject)) 
				return;
			(document as IDataObject).Id = id.ToString();
		}
	}
}