using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Data
{
    public abstract class InMemoryProvider<TObject> : IInitializable
    {
        private readonly List<TObject> _objects = new List<TObject>();

        protected IEnumerable<T> GetObjects<T>(Func<T, bool> predicate) where T : TObject
        {
            return _objects.Where(i => i.GetType().IsAssignableFrom(typeof(T)))
                .Select(i => (T)i)
                .Where(predicate);
        }

        protected void AddObject(TObject obj)
        {
            _objects.Add(obj);
        }

        public void Initialize(Application app)
        {
            OnInitialized(app);
        }
        protected virtual void OnInitialized(Application app)
        {
        }

        public void Shutdown()
        {
            OnShutttingDown();
        }
        protected virtual void OnShutttingDown()
        {
        }
    }
}
