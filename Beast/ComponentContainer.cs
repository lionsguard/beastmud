using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast
{
    internal class ComponentContainer : IInitializable, IUpdatable
    {
        [ImportMany(typeof(IModule))]
        private IEnumerable<Lazy<IModule, IModuleMetadata>> _modules;

        [ImportMany(typeof(IInitializable))]
        private IEnumerable<IInitializable> _initializables;

        [ImportMany(typeof(IUpdatable))]
        private IEnumerable<IUpdatable> _updatables;

        private List<IModule> _innerModules = new List<IModule>();
        public IEnumerable<IModule> Modules
        {
            get { return _innerModules; }
        }

        private void InitCollections()
        {
            if (_modules == null)
                _modules = new List<Lazy<IModule, IModuleMetadata>>();
            if (_initializables == null)
                _initializables = new List<IInitializable>();
            if (_updatables == null)
                _updatables = new List<IUpdatable>();
        }

        public void Compose(Application app, CompositionContainer container)
        {
            InitCollections();

            foreach (var mod in _modules)
            {
                try
                {
                    var module = mod.Value;
                    if (module == null)
                        continue;

                    Trace.TraceInformation("COMPOSITION: Module '{0}'", mod.Metadata.Name);

                    module.App = app;
                    container.ComposeParts(module);

                    _innerModules.Add(module);
                }
                catch (Exception ex)
                {
                    Trace.TraceError("COMPOSITION ERROR: [{0}] {1}", mod.Metadata.Name, ex);
                }
            }
        }

        public void Initialize()
        {
            foreach (var initializable in _initializables)
            {
                initializable.Initialize();
            }
        }

        public void Shutdown()
        {
            foreach (var initializable in _initializables)
            {
                initializable.Shutdown();
            }
        }

        public void Update(ApplicationTime gameTime)
        {
            foreach (var updatable in _updatables)
            {
                updatable.Update(gameTime);
            }
        }
    }
}
