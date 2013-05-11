using Beast.IO;
using System;

namespace Beast
{
    /// <summary>
    /// Provides an abstract implementation of the IConnection interface
    /// </summary>
    public abstract class ConnectionBase : IConnection
    {
        /// <summary>
        /// Gets or sets a unique identifier for the connection.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the tick on which the last activity occured on this connection.
        /// </summary>
        public long LastActivityTick { get; set; }

        /// <summary>
        /// Initializes a new instance of the ConnectionBase class.
        /// </summary>
        protected ConnectionBase()
        {
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Writes output to the current connection.
        /// </summary>
        /// <param name="output">The output to write to the connection.</param>
        public abstract void Write(IOutput output);

        /// <summary>
        /// Closes the connection.
        /// </summary>
        public virtual void Close()
        {
        }
    }
}
