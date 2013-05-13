using Beast;
using Beast.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beast.Commands;
using System.ComponentModel.Composition;
using Beast.Text;

namespace Beast.Samples.Lib
{
    [ExportModule("Sample World")]
    public class SampleWorld : CommandModuleBase
    {
        public override void Initialize(Application app)
        {
            base.Initialize(app);

            // This text parser splits a sentence into a command and arguments.
            DependencyResolver.Register<ITextParser>(() => new CommandTextParser(app));

            // Setup some input converters to convert from text to commands.
            var inputConverter = new TextInputConverter();
            InputResolver.Register<byte[]>(inputConverter); // This type of converter culd be used to sockets
            InputResolver.Register<string>(inputConverter); // This one for web based text input
        }

        public override void Shutdown()
        {
        }

        public override void Update(ApplicationTime time)
        {
        }
    }
}
