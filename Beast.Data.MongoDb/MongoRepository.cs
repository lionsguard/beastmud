
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using Beast.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Beast.Data
{
	[Export(typeof(IRepository))]
	public class MongoRepository : IRepository
	{
		public static readonly ConfigurationProperty ConfigKeyConnectionString =new ConfigurationProperty("connectionString", typeof(string));
		public static readonly ConfigurationProperty ConfigKeyDatabaseName = new ConfigurationProperty("database", typeof(string));

		public string ConnectionString { get; set; }
		public string DatabaseName { get; set; }

		protected MongoServer Server { get; private set; }
		protected MongoDatabase MongoDatabase { get; private set; }

		public void Initialize()
		{
			Server = MongoServer.Create(ConnectionString);
			MongoDatabase = Server.GetDatabase(DatabaseName);

			BsonSerializer.RegisterIdGenerator(typeof(string), StringObjectIdGenerator.Instance);

			RegisterClassMaps();
		}

		public string GetNextObjectId(IGameObject obj)
		{
			return (string)StringObjectIdGenerator.Instance.GenerateId(MongoDatabase.GetCollection<IGameObject>(Collections.Objects), obj);
		}

		protected virtual void RegisterClassMaps()
		{
			
		}

		#region MongoDb Methods
		protected bool SaveMongoObject<T>(T obj, string collectionName) where T : class
		{
			return SaveMongoObject(obj, collectionName, o => o.EQ(PropertyNames.Id));
		}

		protected bool SaveMongoObject<T>(T obj, string collectionName, Func<T, QueryComplete> query) where T : class
		{
			try
			{
				return MongoDatabase.GetCollection<T>(collectionName).Save(obj, SafeMode.True).Ok;
			}
			catch (Exception ex)
			{
				Log.Error(ex);
				return false;
			}
		}

		protected T GetMongoObject<T>(string collectionName, string id) where T : class
		{
			return string.IsNullOrEmpty(id) ? default(T) : MongoDatabase.GetCollection<T>(collectionName).FindOneAs<T>(Query.EQ(PropertyNames.Id.ColumnName, id));
		}

		protected T GetMongoObject<T>(string collectionName, IMongoQuery query) where T : class
		{
			return MongoDatabase.GetCollection<T>(collectionName).FindOneAs<T>(query);
		}

		protected IEnumerable<T> GetMongoObjects<T>(string collectionName, IMongoQuery query) where T : class
		{
			IEnumerable<T> objects = null;
			if (query == null)
				objects = MongoDatabase.GetCollection<T>(collectionName).FindAllAs<T>().ToArray();
			if (objects == null)
				objects = MongoDatabase.GetCollection<T>(collectionName).FindAs<T>(query);
			return objects;
		}

		protected IEnumerable<T> GetMongoObjects<T>(string collectionName) where T : class
		{
			return GetMongoObjects<T>(collectionName, null);
		}

		protected bool DeleteMongoObject<T>(T obj, string collectionName) where T : class
		{
			DeleteMongoObject(obj, collectionName, obj.EQ(PropertyNames.Id));
			return true;
		}

		protected bool DeleteMongoObject<T>(T obj, string collectionName, IMongoQuery query) where T : class
		{
			MongoDatabase.GetCollection<T>(collectionName).Remove(query);
			return true;
		}
		#endregion

		public RepositoryElement ToConfig()
		{
			var config = new RepositoryElement
			       	{
						Type = GetType().AssemblyQualifiedName
			       	};
			config[ConfigKeyConnectionString] = ConnectionString;
			config[ConfigKeyDatabaseName] = DatabaseName;
			return config;
		}

		public void FromConfig(RepositoryElement config)
		{
			ConnectionString = (string) config[ConfigKeyConnectionString];
			DatabaseName = (string) config[ConfigKeyDatabaseName];
		}

		#region Internal Classes
		public class Collections
		{
			public const string CommandDefinitions = "commands";
			public const string Zones = "zones";
			public const string Places = "places";
			public const string Objects = "objects";
		}

		public class PropertyNames
		{
			public static readonly PropertyName Id = new PropertyName("Id", "_id");
			public static readonly PropertyName Name = new PropertyName("Name", "Name");
		}
		#endregion
	}

	public struct PropertyName
	{
		public string ObjectName;
		public string ColumnName;

		public PropertyName(string objectName, string columnName)
		{
			ObjectName = objectName;
			ColumnName = columnName;
		}
	}

	public static class QueryExtensions
	{
		public static QueryComplete EQ<T>(this T obj, PropertyName propertyName)
		{
			return Query.EQ(propertyName.ColumnName, BsonValue.Create(obj.GetValue(propertyName.ObjectName)));
		}
	}
}
