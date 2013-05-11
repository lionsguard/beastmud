using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Hosting.Self
{
    public class SelfHost : IHost
    {
        private Application _app;

        public bool IsRunning
        {
            get { return _app.IsRunning; }
        }

        public void Start(ApplicationSettings settings)
        {
            _app = new Application(this, settings);
            _app.Run();
        }

        public void Stop()
        {
            _app.Dispose();
        }
    }
}
