using Beast.IO;

namespace Beast.Commands
{
    public abstract class CommandModuleBase : IModule
    {
        public Application App { get; set; }

        public bool CanProcessInput(IInput input)
        {
            var cmdName = App.GetCommandName(input);
            return !string.IsNullOrEmpty(cmdName);
        }

        public void ProcessInput(IConnection connection, IInput input)
        {
            App.ExecuteCommand(connection, input);
        }

        public abstract void Initialize();

        public abstract void Shutdown();

        public abstract void Update(ApplicationTime time);
    }
}
