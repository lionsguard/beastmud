using System;
using System.Collections.Generic;

namespace Beast.IO
{
    /// <summary>
    /// Providers static members for resolving raw input.
    /// </summary>
    public static class InputResolver
    {
        private static readonly Dictionary<Type, IInputConverter> _converters = new Dictionary<Type, IInputConverter>();

        /// <summary>
        /// Uses the IInputConverter registered with the DependencyResolver to convert the raw data into an IInput instance.
        /// </summary>
        /// <param name="data">The raw data to convert.</param>
        /// <returns>An IInput instance representing the specified raw data.</returns>
        public static IInput Resolve<T>(T data)
        {
            IInputConverter converter;
            if (!_converters.TryGetValue(typeof(T), out converter))
                converter = new DefaultInputConverter();

            return converter.ConvertInput(data);
        }

        /// <summary>
        /// Registers an IInputConverter for the specified T type.
        /// </summary>
        /// <typeparam name="T">The data in which the specified converter will be used.</typeparam>
        /// <param name="converter">The converter to use for the specified type.</param>
        public static void Register<T>(IInputConverter converter)
        {
            _converters[typeof(T)] = converter;
        }
    }
}
