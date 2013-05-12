using System;

namespace Beast
{
    public interface IDependencyResolver
    {
        void Register<T>(Func<object> activator);
        T Resolve<T>();
    }
}
