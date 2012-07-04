using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Beast.Items;
using Beast.Mobiles;
using Beast.Security;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Beast.Data
{
	[Export(typeof(IUserRepository))]
	[Export(typeof(ITemplateRepository))]
	[Export(typeof(IPlaceRepository))]
	[Export(typeof(ICharacterRepository))]
	public class MongoRepository : IUserRepository, ITemplateRepository, IPlaceRepository, ICharacterRepository
	{
		[Import(ConfigKeys.ConnectionString)]
		public string ConnectionString { get; set; }

		[Import(ConfigKeys.DatabaseName)]
		public string DatabaseName { get; set; }

		protected MongoServer Server { get; private set; }
		protected MongoDatabase MongoDatabase { get; private set; }

		public void Initialize()
		{
			Server = MongoServer.Create(ConnectionString);
			MongoDatabase = Server.GetDatabase(DatabaseName);

			BsonSerializer.RegisterSerializationProvider(new GameObjectSerializationProvider());

			BsonSerializer.RegisterIdGenerator(typeof(string), StringObjectIdGenerator.Instance);

			RegisterClassMap<GameObject>();
			RegisterClassMap<Mobile>();
			RegisterClassMap<Character>();
			RegisterClassMap<Item>();
			RegisterClassMap<Terrain>();
			RegisterClassMap<Place>();
			RegisterClassMap<Login>();
			RegisterClassMap<GenericLogin>();
			RegisterClassMap<User>();

			RegisterClassMaps();

			EnsureIndexes();
		}

		public void Shutdown()
		{
			Server.Disconnect();
		}

		private void EnsureIndexes()
		{
			var keys = IndexKeys.Ascending(PropertyNames.Name.ColumnName);
			var options = IndexOptions.SetUnique(true);
			MongoDatabase.GetCollection<IGameObject>(Collections.Templates).EnsureIndex(keys, options);

			keys = IndexKeys.Ascending(PropertyNames.UserId.ColumnName);
			MongoDatabase.GetCollection<Character>(Collections.Characters).EnsureIndex(keys);
		}

		public IEnumerable<Terrain> GetTerrain()
		{
			return GetMongoObjects<Terrain>(Collections.Terrain);
		}

		public void SaveTerrain(Terrain terrain)
		{
			SaveMongoObject(terrain, Collections.Terrain);
		}

		public long GetTemplateCount()
		{
			return GetMongoObjectCount<IGameObject>(Collections.Templates);
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
			return GetMongoObjectCount<User>(Collections.Users);
		}

		public User GetUser(string username)
		{
			return GetMongoObject<User>(Collections.Users, Query.ElemMatch(PropertyNames.Logins.ColumnName,
								   Query.EQ(PropertyNames.UserName.ColumnName, username)));
		}

		public User GetUserById(string id)
		{
			return GetMongoObject<User>(Collections.Users, id);
		}

		public void SaveUser(User user)
		{
			foreach (var login in user.Logins)
			{
				var existing = GetUser(login.UserName);
				if (existing != null)
				{
					user.Id = existing.Id;
					SaveMongoObject(user, Collections.Users, u => Query.EQ(PropertyNames.Id.ColumnName, u.Id));
					return;
				}
			}
			SaveMongoObject(user, Collections.Users);
		}

		public long GetPlaceCount()
		{
			return GetMongoObjectCount<Place>(Collections.Places);
		}

		public Place GetPlace(string id)
		{
			return GetMongoObject<Place>(Collections.Places, id);
		}

		public void SavePlace(Place place)
		{
			SaveMongoObject(place, Collections.Places);
		}

		public long GetCharacterCount()
		{
			return MongoDatabase.GetCollection<Character>(Collections.Characters).Count();
		}

		public IEnumerable<Character> GetCharacters(string userId)
		{
			return GetMongoObjects<Character>(Collections.Characters, Query.EQ(PropertyNames.UserId.ColumnName, userId));
		}

		public Character GetCharacter(string id)
		{
			return GetMongoObject<Character>(Collections.Characters, id);
		}

		public void SaveCharacter(Character character)
		{
			SaveMongoObject(character, Collections.Characters);
		}

		protected virtual void RegisterClassMaps()
		{
			
		}

		protected void RegisterClassMap<T>()
		{
			if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
				BsonClassMap.RegisterClassMap<T>();
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

		protected long GetMongoObjectCount<T>(string collectionName) where T : class
		{
			return MongoDatabase.GetCollection<T>(collectionName).Count();
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

		#region Internal Classes
		public class Collections
		{
			public const string Places = "places";
			public const string Templates = "templates";
			public const string Characters = "characters";
			public const string Users = "users";
			public const string Terrain = "terrain";
		}

		public class PropertyNames
		{
			public static readonly PropertyName Id = new PropertyName(CommonProperties.Id, "_id");
			public static readonly PropertyName Name = new PropertyName(CommonProperties.Name);
			public static readonly PropertyName Logins = new PropertyName("Logins");
			public static readonly PropertyName UserName = new PropertyName("UserName");
			public static readonly PropertyName UserId = new PropertyName("UserId");
		}
		#endregion
	}

	public struct PropertyName
	{
		public string ObjectName;
		public string ColumnName;

		public PropertyName(string objectName)
			: this(objectName, objectName)
		{
		}

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
