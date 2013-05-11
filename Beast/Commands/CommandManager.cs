using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Commands
{
    internal class CommandManager : IPartImportsSatisfiedNotification
    {
		public event EventHandler<ApplicationErrorEventArgs> Error = delegate{};
	
        [ImportMany(AllowRecomposition=true)]
        private IEnumerable<Lazy<ICommand, ICommandMetadata>> ImportedCommands {get;set;}

        private readonly CommandCollection Commands = new CommandCollection();

        public ICommand GetCommand(string commandName)
        {
            return Commands.Get(commandName);
        }

        private class CommandCollection
        {
            private readonly Dictionary<string, ICommand> _commands = new Dictionary<string, ICommand>(StringComparer.InvariantCultureIgnoreCase);

            public void Add(ICommand command, params string[] aliases)
            {
                foreach (var alias in aliases)
                {
                    _commands[alias] = command;
                }
            }

            public ICommand Get(string name)
            {
                ICommand cmd;
                _commands.TryGetValue(name, out cmd);
                return cmd;
            }
        }

        public void OnImportsSatisfied()
        {
            foreach (var cmd in ImportedCommands)
            {
                cmd.Value.Error += Error;
                Commands.Add(cmd.Value, cmd.Metadata.Aliases);
            }
        }
    }
}
