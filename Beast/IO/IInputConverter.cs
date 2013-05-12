
namespace Beast.IO
{
    /// <summary>
    /// Represents a converter to transform various data into an IInput instance.
    /// </summary>
    public interface IInputConverter
    {
        /// <summary>
        /// Converts the specified data to an IInput instance.
        /// </summary>
        /// <param name="data">The data to convert to an IInput instance.</param>
        /// <returns>An IInput instance representing the converted data.</returns>
        IInput ConvertInput<T>(T data);
    }
}
