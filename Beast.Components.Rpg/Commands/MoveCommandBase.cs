using Beast.IO;
using Beast.Mapping;
using System.Collections.Generic;

namespace Beast.Commands
{
    [ExportCommand("move", "n", "s", "e", "w", "ne", "nw", "se", "sw", "u", "d", "north", "south", "east", "west", "northeast", "northwest", "southeast", "southwest", "up", "down")]
    public abstract class MoveCommandBase : CommandBase
    {
        public override IEnumerable<string> ArgumentNames
        {
            get { return new []{"direction"}; }
        }

        protected override void ExecuteOverride(string alias, IConnection connection, IInput input, IOutput output)
        {
            output.Command = "move";

            var cmd = input.Get(CommandSettingsKeys.DefaultCommandNameValue, string.Empty);

            if (!string.IsNullOrEmpty(cmd))
            {
                if (cmd.ToLowerInvariant() == "move")
                {
                    foreach (var arg in input)
                    {
                        if (arg.Key != CommandSettingsKeys.DefaultCommandNameValue)
                        {
                            cmd = arg.Value.ToString();
                            break;
                        }
                    }
                }
            }

            var direction = Direction.FromAlias(cmd);
            if (direction.Unit == Unit.Zero)
            {
                OnInvalidMove(cmd, connection, input, output);
                return;
            }

            if (!CanMove(direction, connection, input, output))
            {
                OnInvalidMove(direction.Value.ToString(), connection, input, output);
                return;
            }

            DoMove(direction, connection, input, output);
        }

        protected abstract bool CanMove(Direction direction, IConnection connection, IInput input, IOutput output);

        protected abstract void DoMove(Direction direction, IConnection connection, IInput input, IOutput output);

        protected abstract void OnInvalidMove(string direction, IConnection connection, IInput input, IOutput output);
    }
}
