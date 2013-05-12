using Beast.Commands;
using Beast.IO;

namespace Beast.Samples.Lib.Commands
{
    [ExportCommand(CommandSettingsKeys.CatchAllCommandName)]
    public class EchoCommand : CommandBase
    {
        protected override IOutput CreateOutput(IInput input)
        {
            return new BasicOutput(input.Id);
        }

        protected override void ExecuteOverride(IConnection connection, IInput input, IOutput output)
        {
            output.Invalidate(string.Format("ECHO: {0}", input.ToString()));
        }
    }
}
