using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast
{
    public abstract class WorldBase : ModuleBase
    {
        public override bool CanProcessInput(IO.IInput input)
        {
            return false;
        }

        public override void ProcessInput(IConnection connection, IO.IInput input)
        {
        }

        public override void Initialize(Application app)
        {
            base.Initialize(app);

            OnInitialized(app);
        }
        protected abstract void OnInitialized(Application app);

        public override void Shutdown()
        {
            OnShutdown();
        }
        protected abstract void OnShutdown();

        public override void Update(ApplicationTime time)
        {
            OnUpdate(time);
        }
        protected abstract void OnUpdate(ApplicationTime time);
    }
}
