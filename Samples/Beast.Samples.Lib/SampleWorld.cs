using Beast;
using Beast.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beast.Commands;
using System.ComponentModel.Composition;

namespace Beast.Samples.Lib
{
    [ExportModule("Sample World")]
    public class SampleWorld : CommandModuleBase
    {
        public override void Initialize()
        {
        }

        public override void Shutdown()
        {
        }

        public override void Update(ApplicationTime time)
        {
        }
    }
}
