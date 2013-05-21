
namespace Beast.Behaviors
{
    public interface IBehavior
    {
        IGameObject Owner { get; }
        void Attach(IGameObject owner);
        void Detach();
    }
}
