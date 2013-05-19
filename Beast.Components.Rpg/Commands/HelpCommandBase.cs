using Beast.Commands;
using Beast.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Commands
{
    [ExportCommand("?", "help")]
    public abstract class HelpCommandBase : CommandBase
    {
        public const string ParameterNameCommand = "command";

        public override IEnumerable<string> ArgumentNames
        {
            get { return new[] { ParameterNameCommand }; }
        }

        protected override void ExecuteOverride(string alias, IConnection connection, IInput input, IOutput output)
        {
            var cmdName = input.Get(ParameterNameCommand, string.Empty);

            if (string.IsNullOrEmpty(cmdName))
            {
                OnInvalidHelpArgs(cmdName, connection, input, output);
                return;
            }

            var cmd = DependencyResolver.Resolve<IWorld>().App.GetCommand(cmdName);
            if (cmd == null)
            {
                OnCommandNotFound(cmdName, connection, input, output);
                return;
            }

            output.Data = new NotificationOutput(input.Id, 0, cmd.HelpText);
        }

        protected abstract void OnInvalidHelpArgs(string commandName, IConnection connection, IInput input, IOutput output);
        protected abstract void OnCommandNotFound(string commandName, IConnection connection, IInput input, IOutput output);
    }
}
