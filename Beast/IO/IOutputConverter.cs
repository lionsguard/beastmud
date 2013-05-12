
namespace Beast.IO
{
    /// <summary>
    /// Represents an converter that transforms IOutput to other data types.
    /// </summary>
    public interface IOutputConverter
    {
        /// <summary>
        /// Converts the specified IOutput to another type.
        /// </summary>
        /// <param name="output">The output to convert.</param>
        /// <returns>An object representing the converted output.</returns>
        object ConvertOutput(IOutput output);
    }
}
