using System.Text;
using Newtonsoft.Json;

namespace Beast.IO
{
    /// <summary>
    /// Provides an implementation of the IOutput interface.
    /// </summary>
    public class BasicOutput : IOutput
    {
        /// <summary>
        /// Gets the unique identifier for the output. In the case of an input response this may reflect the input identifier.
        /// </summary>
        public string Id { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the output contains a successful response.
        /// </summary>
        public bool Ok { get; set; }

        /// <summary>
        /// Gets or sets an error message associated with the output, if the Ok value is set to false.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the data for the output or response.
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Initializes a new instance of the BasicOutput class and sets the Id to the specified input id.
        /// </summary>
        /// <param name="inputId">The id of the input.</param>
        public BasicOutput(string inputId)
        {
            Id = Id;
        }

        /// <summary>
        /// Invaliates the output and providers an error message.
        /// </summary>
        /// <param name="errorMessage">The text containing the reason for the invalid output.</param>
        /// <returns>The current IOutput instance.</returns>
        public IOutput Invalidate(string errorMessage)
        {
            Ok = false;
            ErrorMessage = errorMessage;
            return this;
        }

        /// <summary>
        /// Converts the output to a byte array.
        /// </summary>
        /// <returns>A byte array representing the current output.</returns>
        public virtual byte[] ToBytes()
        {
            return Encoding.ASCII.GetBytes(ToString());
        }

        /// <summary>
        /// Converts the output to a string.
        /// </summary>
        /// <returns>A string representing the current output.</returns>
        public override string ToString()
        {
            if (Data == null)
                return string.Empty;
            return Data.ToString();
        }

        /// <summary>
        /// Converts the output to a JSON string.
        /// </summary>
        /// <returns>A JSON string representing the current output.</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
