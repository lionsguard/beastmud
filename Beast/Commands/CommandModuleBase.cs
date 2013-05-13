using Beast.IO;

namespace Beast.Commands
{
    public abstract class CommandModuleBase : ModuleBase
    {
        public override bool CanProcessInput(IInput input)
        {
            var cmdName = App.GetCommandName(input);
            return !string.IsNullOrEmpty(cmdName);
        }

        public override void ProcessInput(IConnection connection, IInput input)
        {
            App.ExecuteCommand(connection, input);
        }
    }
}
