
namespace Beast.Text
{
    /// <summary>
    /// Defines a parser that can convert raw string input into a TextInput instance.
    /// </summary>
    public interface ITextParser
    {
        /// <summary>
        /// Parses the specified input into a TextInput instance.
        /// </summary>
        /// <param name="input">Raw string input to convert.</param>
        /// <returns>A TextInput instance for the parsed input value.</returns>
        TextInput Parse(string input);
    }
}
