
namespace Beast.IO
{
    /// <summary>
    /// Represents output from the BeastMUD Application.
    /// </summary>
    public interface IOutput
    {
        /// <summary>
        /// Gets the unique identifier for the output. In the case of an input response this may reflect the input identifier.
        /// </summary>
        string Id { get; }
		
        /// <summary>
        /// Gets or sets a value indicating whether or not the output contains a successful response.
        /// </summary>
		bool Ok {get;set;}
		
        /// <summary>
        /// Gets or sets an error message associated with the output, if the Ok value is set to false.
        /// </summary>
		string ErrorMessage {get; set;}

        /// <summary>
        /// Gets or sets the data for the output or response.
        /// </summary>
        object Data { get; set; }

        /// <summary>
        /// Invaliates the output and providers an error message.
        /// </summary>
        /// <param name="errorMessage">The text containing the reason for the invalid output.</param>
        /// <returns>The current IOutput instance.</returns>
        IOutput Invalidate(string errorMessage);

        /// <summary>
        /// Converts the output to a byte array.
        /// </summary>
        /// <returns>A byte array representing the current output.</returns>
        byte[] ToBytes();

        /// <summary>
        /// Converts the output to a string.
        /// </summary>
        /// <returns>A string representing the current output.</returns>
        string ToString();

        /// <summary>
        /// Converts the output to a JSON string.
        /// </summary>
        /// <returns>A JSON string representing the current output.</returns>
        string ToJson();
    }
}
