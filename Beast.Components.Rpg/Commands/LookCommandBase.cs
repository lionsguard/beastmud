using Beast.Commands;
using Beast.IO;
using Beast.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Commands
{
    [ExportCommand(CommandNameLook, CommandNameGlance, CommandNameExamine)]
    public abstract class LookCommandBase : CommandBase
    {
        public const string CommandNameLook = "look";
        public const string CommandNameGlance = "glance";
        public const string CommandNameExamine = "examine";

        public const string ParameterNameDirection = "direction";
        public const string ParameterNameObject = "direction";

        public override IEnumerable<string> ArgumentNames
        {
            get { return new[] { ParameterNameDirection, ParameterNameObject }; }
        }

        protected override void ExecuteOverride(string alias, IConnection connection, IInput input, IOutput output)
        {
            var dirName = input.Get(ParameterNameDirection, string.Empty);
            var objName = input.Get(ParameterNameObject, string.Empty);
            if (string.IsNullOrEmpty(objName) && !string.IsNullOrEmpty(dirName))
                objName = dirName;

            var loweredAlias = alias.ToLowerInvariant();

            if (!string.IsNullOrEmpty(dirName) && (loweredAlias == CommandNameLook || loweredAlias == CommandNameGlance))
            {
                Direction dir;
                if (Direction.TryGetDirection(dirName, out dir))
                {
                    LookInDirection(dir, connection, input, output);
                    return;
                }
            }

            if (!string.IsNullOrEmpty(objName) && loweredAlias == CommandNameExamine)
            {
                ExamineObject(objName, connection, input, output);
                return;
            }

            // Default to normal LOOK at the current location.
            Look(connection, input, output);
        }

        protected abstract void LookInDirection(Direction direction, IConnection connection, IInput input, IOutput output);
        protected abstract void ExamineObject(string name, IConnection connection, IInput input, IOutput output);
        protected abstract void Look(IConnection connection, IInput input, IOutput output);
    }
}
