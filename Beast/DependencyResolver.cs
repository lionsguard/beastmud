using System;
using System.Collections.Generic;
using Beast.Text;

namespace Beast
{
    public static class DependencyResolver
    {
        private static IDependencyResolver _resolver;
        private static Type _defaultKey;

        static DependencyResolver()
        {
            _resolver = new DefaultDependencyResolver();
            _defaultKey = typeof(DependencyResolver);

            Register<ITextParser>(() => new BasicTextParser());
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
    }

    internal class DefaultDependencyResolver : IDependencyResolver
    {
        private readonly Dictionary<Type, Func<object>> _instances = new Dictionary<Type, Func<object>>();

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
