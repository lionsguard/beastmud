using System;
using System.Collections.Generic;

namespace Beast
{
    public static class DependencyResolver
    {
        private static IDependencyResolver _resolver;

        static DependencyResolver()
        {
            _resolver = new DefaultDependencyResolver();
        }

        public static void Register<T>(Func<object> activator)
        {
            _resolver.Register<T>(activator);
        }

        public static T Resolve<T>()
        {
            return _resolver.Resolve<T>();
        }

        public static void SetResolver(IDependencyResolver resolver)
        {
            _resolver = resolver;
        }

        private class DefaultDependencyResolver : IDependencyResolver
        {
            private readonly Dictionary<Type, Func<object>> _instances = new Dictionary<Type, Func<object>>();

            public DefaultDependencyResolver()
            {
            }

            public void Register<T>(Func<object> activator)
            {
                _instances[typeof(T)] = activator;
            }

            public T Resolve<T>()
            {
                Func<object> activator;
                if (!_instances.TryGetValue(typeof(T), out activator))
                    return default(T);
                return (T)activator();
            }
        }
    }
}
