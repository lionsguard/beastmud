
namespace Beast.Behaviors
{
	public abstract class Behavior
	{
		public GameObject Owner { get; private set; }

		public void Attach(GameObject owner)
		{
			Owner = owner;
			OnAttached();
		}

		public void Detach()
		{
			Owner = null;
			OnDetaching();
		}

		protected virtual void OnAttached()
		{
			
		}
		protected virtual void OnDetaching()
		{
			
		}
	}

	public abstract class Behavior<T> : Behavior where T : GameObject
	{
		public new T Owner
		{
			get
			{
				return (T)base.Owner;
			}
		}
	}
}
