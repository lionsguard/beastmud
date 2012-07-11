using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
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
	[Export(typeof(IWorldRepository))]
	public class MongoRepository : IUserRepository, ITemplateRepository, IPlaceRepository, ICharacterRepository, IWorldRepository
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

			BsonSerializer.RegisterSerializationProvider(new DataObjectSerializationProvider());
			BsonSerializer.RegisterIdGenerator(typeof(string), StringObjectIdGenerator.Instance);

			EnsureClassMaps();

			EnsureIndexes();
		}

		public void Shutdown()
		{
			Server.Disconnect();
		}

		private void EnsureClassMaps()
		{
			foreach (var type in Game.Current.GetKnownTypes())
			{
				RegisterClassMap(type);	
			}
		}

		private void EnsureIndexes()
		{
			var keys = IndexKeys.Ascending(PropertyNames.Name.ColumnName);
			var options = IndexOptions.SetUnique(true);
			MongoDatabase.GetCollection<IGameObject>(Collections.Templates).EnsureIndex(keys, options);

			keys = IndexKeys.Ascending(PropertyNames.UserId.ColumnName);
			MongoDatabase.GetCollection<Character>(Collections.Characters).EnsureIndex(keys);

			keys = IndexKeys.Ascending(PropertyNames.Name.ColumnName);
			options = IndexOptions.SetUnique(true);
			MongoDatabase.GetCollection<Character>(Collections.Characters).EnsureIndex(keys, options);
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

		public Character GetCharacterByName(string name)
		{
			return GetMongoObject<Character>(Collections.Characters, Query.EQ(PropertyNames.Name.ColumnName, name));
		}

		public void SaveCharacter(Character character)
		{
			SaveMongoObject(character, Collections.Characters);
		}

		public IWorld GetWorld()
		{
			return MongoDatabase.GetCollection<IWorld>(Collections.World).FindOne();
		}

		public void SaveWorld<T>(T world) where T : class, IWorld
		{
			SaveMongoObject(world, Collections.World);
		}

		#region MongoDb Methods

		protected void RegisterClassMap(Type type)
		{
			if (!BsonClassMap.IsClassMapRegistered(type))
				BsonClassMap.RegisterClassMap(new GenericClassMap(type));
		}

		protected bool SaveMongoObject<T>(T obj, string collectionName) where T : class, IDataObject
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

		protected bool SaveMongoObject<T>(T obj, string collectionName, Func<T, IMongoQuery> query) where T : class, IDataObject
		{
			try
			{
				return MongoDatabase.GetCollection<T>(collectionName).Update(query(obj), Update.Replace(obj), UpdateFlags.Upsert, SafeMode.True).Ok;
			}
			catch (Exception ex)
			{
				Log.Error(ex);
				return false;
			}
		}

		protected long GetMongoObjectCount<T>(string collectionName) where T : class, IDataObject
		{
			return MongoDatabase.GetCollection<T>(collectionName).Count();
		}

		protected T GetMongoObject<T>(string collectionName, string id) where T : class, IDataObject
		{
			return string.IsNullOrEmpty(id) ? default(T) : MongoDatabase.GetCollection<T>(collectionName).FindOneAs<T>(Query.EQ(PropertyNames.Id.ColumnName, id));
		}

		protected T GetMongoObject<T>(string collectionName, IMongoQuery query) where T : class, IDataObject
		{
			return MongoDatabase.GetCollection<T>(collectionName).FindOneAs<T>(query);
		}

		protected IEnumerable<T> GetMongoObjects<T>(string collectionName, IMongoQuery query) where T : class, IDataObject
		{
			IEnumerable<T> objects = null;
			if (query == null)
				objects = MongoDatabase.GetCollection<T>(collectionName).FindAllAs<T>().ToArray();
			if (objects == null)
				objects = MongoDatabase.GetCollection<T>(collectionName).FindAs<T>(query);
			return objects;
		}

		protected IEnumerable<T> GetMongoObjects<T>(string collectionName) where T : class, IDataObject
		{
			return GetMongoObjects<T>(collectionName, null);
		}

		protected bool DeleteMongoObject<T>(T obj, string collectionName) where T : class, IDataObject
		{
			DeleteMongoObject(obj, collectionName, obj.EQ(PropertyNames.Id));
			return true;
		}

		protected bool DeleteMongoObject<T>(T obj, string collectionName, IMongoQuery query) where T : class, IDataObject
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
			public const string World = "world";
		}

		public class PropertyNames
		{
			public static readonly PropertyName Id = new PropertyName(CommonProperties.Id, "_id");
			public static readonly PropertyName Name = new PropertyName(CommonProperties.Name);
			public static readonly PropertyName Logins = new PropertyName("Logins");
			public static readonly PropertyName UserName = new PropertyName("UserName");
			public static readonly PropertyName UserId = new PropertyName("UserId");
		}

		public class GenericClassMap : BsonClassMap
		{
			public GenericClassMap(Type classType) : base(classType)
			{
				SetIdMember(new BsonMemberMap(this, classType.GetMember("Id").FirstOrDefault()));
			}
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
		public static IMongoQuery EQ<T>(this T obj, PropertyName propertyName)
		{
			return Query.EQ(propertyName.ColumnName, BsonValue.Create(obj.GetValue(propertyName.ObjectName)));
		}
	}
}
