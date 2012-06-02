
using System;
using Beast.Behaviors;

namespace Beast
{
	/// <summary>
	/// Represents an interactive game object or object used explicitly within the game world and updated by the game engine.
	/// </summary>
	public class GameObject : IUpdatable
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public BehaviorCollection Behaviors { get; set; }
		public PropertyCollection Properties { get; set; }

		#region Events
		public event EventHandler Updating = delegate { };
		public event EventHandler Updated = delegate { };

		//public event EventHandler Created = delegate { };
		//public event EventHandler Destroyed = delegate { }; 
		#endregion

		public GameObject()
		{
			Behaviors = new BehaviorCollection(this);
			Properties = new PropertyCollection();
		}

		public void Update(GameTime gameTime)
		{
			Updating(this, EventArgs.Empty);
			UpdateOverride(gameTime);
			Updated(this, EventArgs.Empty);
		}
		protected virtual void UpdateOverride(GameTime gameTime)
		{
		}
	}
}
