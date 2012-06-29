using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using Beast.Configuration;
using Beast.Security;
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
		public static readonly ConfigurationProperty ConfigKeyConnectionString = new ConfigurationProperty("connectionString", typeof(string));
		public static readonly ConfigurationProperty ConfigKeyDatabaseName = new ConfigurationProperty("database", typeof(string));

		public string ConnectionString { get; set; }
		public string DatabaseName { get; set; }

		protected MongoServer Server { get; private set; }
		protected MongoDatabase MongoDatabase { get; private set; }

		public void Initialize()
		{
			Server = MongoServer.Create(ConnectionString);
			MongoDatabase = Server.GetDatabase(DatabaseName);

			BsonSerializer.RegisterSerializationProvider(new GameObjectSerializationProvider());

			BsonSerializer.RegisterIdGenerator(typeof(string), StringObjectIdGenerator.Instance);

			BsonClassMap.RegisterClassMap<GameObject>();

			BsonClassMap.RegisterClassMap<Login>();
			BsonClassMap.RegisterClassMap<GenericLogin>();
			BsonClassMap.RegisterClassMap<User>();

			RegisterClassMaps();

			EnsureIndexes();
		}

		private void EnsureIndexes()
		{
			var keys = IndexKeys.Ascending(PropertyNames.Name.ColumnName);
			var options = IndexOptions.SetUnique(true);
			MongoDatabase.GetCollection<IGameObject>(Collections.Templates).EnsureIndex(keys, options);

			keys = IndexKeys.Ascending(PropertyNames.X.ColumnName, PropertyNames.Y.ColumnName, PropertyNames.Z.ColumnName);
			options = IndexOptions.SetUnique(true);
			MongoDatabase.GetCollection<Place>(Collections.Places).EnsureIndex(keys, options);
		}

		public IGameObject GetTemplate(string templateName)
		{
			return GetMongoObject<IGameObject>(Collections.Templates, Query.EQ(PropertyNames.Name.ColumnName, templateName));
		}

		public void SaveTemplate(IGameObject obj)
		{
			var existing = GetTemplate(obj.Name);
			if (existing != null)
			{
				obj.Id = existing.Id;
			}
			SaveMongoObject(obj, Collections.Templates);
		}

		public long GetUserCount()
		{
			return MongoDatabase.GetCollection<User>(Collections.Users).Count();
		}

		public User GetUser(Login login)
		{
			return GetMongoObject<User>(Collections.Users, Query.ElemMatch(PropertyNames.Logins.ColumnName,
								   Query.EQ(PropertyNames.UserName.ColumnName, login.UserName)));
		}

		public void SaveUser(User user)
		{
			foreach (var login in user.Logins)
			{
				var existing = GetUser(login);
				if (existing != null)
				{
					user.Id = existing.Id;
					SaveMongoObject(user, Collections.Users, u => Query.EQ(PropertyNames.Id.ColumnName, u.Id));
					return;
				}
			}
			SaveMongoObject(user, Collections.Users);
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
			public const string Places = "places";
			public const string Templates = "templates";
			public const string Characters = "characters";
			public const string Users = "users";
		}

		public class PropertyNames
		{
			public static readonly PropertyName Id = new PropertyName("Id", "_id");
			public static readonly PropertyName Name = new PropertyName("Name", "Name");
			public static readonly PropertyName Logins = new PropertyName("Logins", "Logins");
			public static readonly PropertyName UserName = new PropertyName("UserName", "UserName");
			public static readonly PropertyName X = new PropertyName("X", "X");
			public static readonly PropertyName Y = new PropertyName("Y", "Y");
			public static readonly PropertyName Z = new PropertyName("Z", "Z");
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
