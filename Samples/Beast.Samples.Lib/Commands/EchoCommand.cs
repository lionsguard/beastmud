using Beast.Commands;
using Beast.IO;

namespace Beast.Samples.Lib.Commands
{
    [ExportCommand(CommandSettingsKeys.CatchAllCommandName)]
    public class EchoCommand : CommandBase
    {
        protected override void ExecuteOverride(string alias, IConnection connection, IInput input, IOutput output)
        {
            output.Invalidate(string.Format("ECHO: {0}", input.ToString()));
        }

        public override System.Collections.Generic.IEnumerable<string> ArgumentNames
        {
            get { return new[]{""}; }
        }

        public override string HelpText
        {
            get { return ""; }
        }
    }
}
