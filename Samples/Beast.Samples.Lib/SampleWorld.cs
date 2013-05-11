using Beast.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Samples.Lib
{
    public class SampleWorld : IModule
    {
        public Application App { get; set; }

        public bool CanProcessInput(IInput input)
        {
            return true;
        }

        public void ProcessInput(IConnection connection, IInput input)
        {
        }

        public void Initialize()
        {
        }

        public void Shutdown()
        {
        }

        public void Update(ApplicationTime gameTime)
        {
        }
    }
}
