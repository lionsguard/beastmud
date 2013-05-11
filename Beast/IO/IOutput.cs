using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		
		IOutput Invalidate(string errorMessage);
    }
}
