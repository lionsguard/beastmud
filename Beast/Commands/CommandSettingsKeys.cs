using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Commands
{
    public class CommandSettingsKeys
    {
        /// <summary>
        /// Specifies the default value of the command name parameter.
        /// </summary>
        public const string DefaultCommandNameValue = "Command";

        /// <summary>
        /// Specifies the key for the command name parameter. i.e. Parameter=Value (<add key=CommandName value=Command/>)
        /// </summary>
        public const string CommandNameKey = "CommandName";
    }
}
