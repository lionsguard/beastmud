
namespace Beast.Behaviors
{
    public abstract class Behavior : IBehavior
    {
        public IGameObject Owner { get; private set; }

        public void Attach(IGameObject owner)
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

    public abstract class Behavior<T> : Behavior where T : IGameObject
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
