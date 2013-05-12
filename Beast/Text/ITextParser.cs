using System.Collections.Generic;

namespace Beast.Text
{
    /// <summary>
    /// Defines a parser that can convert raw string input into an array of KeyValuePairs.
    /// </summary>
    public interface ITextParser
    {
        /// <summary>
        /// Parses the specified input into an enumerable KeyValuePair collection.
        /// </summary>
        /// <param name="input">Raw string input to convert.</param>
        /// <returns>An IEnumerable<KeyValuePair<string,object>> instance for the parsed input value.</returns>
        IEnumerable<KeyValuePair<string,object>> Parse(string input);
    }
}
