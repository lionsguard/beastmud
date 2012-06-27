
using System;
using System.Collections;
using System.Collections.Generic;
using Beast.Behaviors;
using Newtonsoft.Json;

namespace Beast
{
	/// <summary>
	/// Represents an interactive game object; an object used explicitly within the game world and updated by the game engine.
	/// </summary>
	public class GameObject : IGameObject
	{
		public string Id
		{
			get { return Get<string>(CommonProperties.Id); }
			set { Set(CommonProperties.Id, value); }
		}
		public string Name
		{
			get { return Get<string>(CommonProperties.Name); }
			set { Set(CommonProperties.Name, value); }
		}
		public string Description
		{
			get { return Get<string>(CommonProperties.Description); }
			set { Set(CommonProperties.Description, value); }
		}

		protected BehaviorCollection Behaviors { get; private set; }

		private readonly Dictionary<Property, object> _properties = new Dictionary<Property, object>();

		public object this[Property property]
		{
			get { return Get(property); }
			set{Set(property, value);}
		}

		public GameObject()
		{
			Id = Game.Current.Repository.GetNextObjectId(this);
			Behaviors = new BehaviorCollection(this);
		}

		#region Property Handling
		public T Get<T>(Property property)
		{
			var value = Get(property);
			if (value == null)
				return default(T);
			if (value.GetType() == typeof(T))
				return (T) value;
			return (T) Convert.ChangeType(value, typeof (T));
		}

		protected internal object Get(Property property)
		{
			object value;
			return !_properties.TryGetValue(property, out value) ? property.DefaultValue : value;
		}
		protected internal void Set(Property property, object value)
		{
			_properties[property] = value;
		}

		public void Merge(IGameObject obj, bool overwriteExisting)
		{
			foreach (var kvp in obj)
			{
				if (!overwriteExisting && _properties.ContainsKey(kvp.Key))
					continue;
				_properties[kvp.Key] = kvp.Value;
			}
		}
		#endregion

		public IEnumerator<KeyValuePair<Property, object>> GetEnumerator()
		{
			return _properties.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public override bool Equals(object obj)
		{
			return obj is IGameObject && (obj as IGameObject).Id.Equals(Id);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public void Update(GameTime gameTime)
		{
			EventManager.Raise(CommonEvents.Updating, this, EventArgs.Empty);
			UpdateOverride(gameTime);
			EventManager.Raise(CommonEvents.Updated, this, EventArgs.Empty);
		}

		/// <summary>
		/// Provides an override where derived classes can perform actions during an object update.
		/// </summary>
		/// <param name="gameTime"></param>
		protected virtual void UpdateOverride(GameTime gameTime)
		{
		}

		public virtual string ToShortString()
		{
			return Name;
		}

		public virtual string ToLongString()
		{
			return string.Concat(Name, Environment.NewLine, Description);
		}
	}

	public class GameObjectJsonConverter : JsonConverter
	{
		#region Overrides of JsonConverter

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType is IGameObject || objectType.IsSubclassOf(typeof (IGameObject)) || objectType.GetInterface(typeof (IGameObject).Name) != null;
		}

		#endregion
	}
}
