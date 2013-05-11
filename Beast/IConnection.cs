using Beast.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast
{
    /// <summary>
    /// Represents a connection to the application.
    /// </summary>
    public interface IConnection
    {
        /// <summary>
        /// Gets or sets a unique identifier for the connection.
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Gets or sets the tick on which the last activity occured on this connection.
        /// </summary>
        long LastActivityTick { get; set; }

        /// <summary>
        /// Writes output to the current connection.
        /// </summary>
        /// <param name="output">The output to write to the connection.</param>
        void Write(IOutput output);

        /// <summary>
        /// Closes the connection.
        /// </summary>
        void Close();
    }
}
