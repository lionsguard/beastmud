using System;

namespace Beast
{
    public abstract class GameObject : IGameObject
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        protected GameObject()
        {
            Id = Guid.NewGuid().ToString();
        }

        public override bool Equals(object obj)
        {
            return (obj is GameObject) && string.Equals((obj as GameObject).Id, Id, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public virtual void Update(ApplicationTime time)
        {
        }
    }
}
