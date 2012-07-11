using System;
using System.Collections.Generic;
using Beast.Behaviors;

namespace Beast
{
	/// <summary>
	/// Represents an interactive game object; an object used explicitly within the game world and updated by the game engine.
	/// </summary>
	public class GameObject : DataObject, IGameObject
	{
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

		public GameObject()
		{
			Behaviors = new BehaviorCollection(this);
			Flags = new FlagCollection(this);
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
	}
}
