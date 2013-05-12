using System.Collections.Generic;
using Beast.Commands;

namespace Beast.Text
{
    public class CommandTextParser : ITextParser
    {
        protected Application App { get; private set; }

        public CommandTextParser(Application app)
        {
            App = app;
        }

        public IEnumerable<KeyValuePair<string, object>> Parse(string input)
        {
            var result = new Dictionary<string, object>();

            var words = input.Split(' ');
            if (words.Length > 0)
            {
                var cmdName = words[0];
                result.Add(App.Settings.GetValue(CommandSettingsKeys.CommandNameKey, CommandSettingsKeys.DefaultCommandNameValue), cmdName);

                if (words.Length > 1)
                {
                    var cmd = App.GetCommand(cmdName);
                    if (cmd != null)
                    {
                        if (cmd.ArgumentNames != null)
                        {
                            var index = 1;
                            foreach (var arg in cmd.ArgumentNames)
                            {
                                if (index >= words.Length)
                                    break;

                                result.Add(arg, words[index]);

                                index++;
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}
