using System;
using System.Collections;
using System.Collections.Generic;
using Beast.Behaviors;

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

		public FlagCollection Flags { get; private set; }
		protected BehaviorCollection Behaviors { get; private set; }

		private readonly Dictionary<string, object> _properties = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

		public object this[string key]
		{
			get { return Get(key); }
			set{Set(key, value);}
		}

		public GameObject()
		{
			Behaviors = new BehaviorCollection(this);
			Flags = new FlagCollection(this);
		}

		#region Property Handling
		public T Get<T>(string property)
		{
			var value = Get(property);
			if (value == null)
				return default(T);
			if (value.GetType() == typeof(T))
				return (T) value;
			if (value is IConvertible)
				return (T) Convert.ChangeType(value, typeof (T));
			return (T) value;
		}

		protected internal object Get(string property)
		{
			object value;
			_properties.TryGetValue(property, out value);
			return value;
		}
		protected internal void Set(string property, object value)
		{
			_properties[property] = value;
		}

		public void Merge(IDictionary<string, object> values, bool overwriteExisting)
		{
			foreach (var kvp in values)
			{
				if (!overwriteExisting && _properties.ContainsKey(kvp.Key))
					continue;

				_properties[kvp.Key] = kvp.Value;
			}
		}
		#endregion

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return _properties.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public override bool Equals(object obj)
		{
			return obj is IGameObject && Equals(obj as IGameObject);
		}

		public bool Equals(IGameObject obj)
		{
			if (string.IsNullOrEmpty(obj.Id) || string.IsNullOrEmpty(Id))
				return base.Equals(obj);
			return obj.Id.Equals(Id);
		}

		public override int GetHashCode()
		{
			return string.IsNullOrEmpty(Id) ? base.GetHashCode() : Id.GetHashCode();
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

		public static T FromTemplate<T>(string templateName, params KeyValuePair<string,object>[] properties) where T : IGameObject
		{
			return FromTemplate<T>(Game.Current.Repository.GetTemplate(templateName));
		}

		public static T FromTemplate<T>(IGameObject template, params KeyValuePair<string, object>[] properties) where T : IGameObject
		{
			if (properties == null)
				properties = new KeyValuePair<string, object>[0];

			var obj = Activator.CreateInstance<T>();
			obj.Merge(template, true);
			foreach (var kvp in properties)
			{
				obj[kvp.Key] = kvp.Value;
			}
			return obj;
		}

		#region Explicit Members
		bool IDictionary<string, object>.ContainsKey(string key)
		{
			return _properties.ContainsKey(key);
		}

		void IDictionary<string, object>.Add(string key, object value)
		{
			Set(key, value);
		}

		bool IDictionary<string, object>.Remove(string key)
		{
			return _properties.Remove(key);
		}

		bool IDictionary<string, object>.TryGetValue(string key, out object value)
		{
			return _properties.TryGetValue(key, out value);
		}

		ICollection<string> IDictionary<string, object>.Keys
		{
			get { return _properties.Keys; }
		}

		ICollection<object> IDictionary<string, object>.Values
		{
			get { return _properties.Values; }
		}

		void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
		{
			Set(item.Key, item.Value);
		}

		void ICollection<KeyValuePair<string, object>>.Clear()
		{
			_properties.Clear();
		}

		bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
		{
			return _properties.ContainsKey(item.Key);
		}

		void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<string,object>>)_properties).CopyTo(array, arrayIndex);
		}

		bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
		{
			return _properties.Remove(item.Key);
		}

		int ICollection<KeyValuePair<string, object>>.Count
		{
			get { return _properties.Count; }
		}

		bool ICollection<KeyValuePair<string, object>>.IsReadOnly
		{
			get { return false; }
		}

		#endregion
	}
}
