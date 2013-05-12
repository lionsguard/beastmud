using System;
using System.Collections.Generic;

namespace Beast.IO
{
    /// <summary>
    /// Providers static members for resolving raw output.
    /// </summary>
    public static class OutputResolver
    {
        private static readonly Dictionary<Type, IOutputConverter> _converters = new Dictionary<Type, IOutputConverter>();

        /// <summary>
        /// Uses the IOutputConverter registered with the DependencyResolver to convert the IOutput instance to raw output.
        /// </summary>
        /// <param name="output">The IOutput instance to convert.</param>
        /// <returns>An object representing the specified IOutput.</returns>
        public static T Resolve<T>(IOutput output)
        {
            IOutputConverter converter;
            if (!_converters.TryGetValue(typeof(T), out converter))
                converter = new DefaultOutputConverter();

            return (T)converter.ConvertOutput(output);
        }

        /// <summary>
        /// Registers an IOutputConverter for the specified T type.
        /// </summary>
        /// <typeparam name="T">The data in which the specified converter will be used.</typeparam>
        /// <param name="converter">The converter to use for the specified type.</param>
        public static void Register<T>(IOutputConverter converter)
        {
            _converters[typeof(T)] = converter;
        }
    }
}
