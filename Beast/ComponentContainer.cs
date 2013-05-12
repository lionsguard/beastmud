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
    internal class ComponentContainer : IInitializable, IUpdatable, IPartImportsSatisfiedNotification
    {
        [ImportMany(AllowRecomposition = true)]
        private IEnumerable<Lazy<IModule, IModuleMetadata>> ImportedModules { get; set; }

        [ImportMany(AllowRecomposition = true)]
        private IEnumerable<IInitializable> ImportedInitializables { get; set; }

        [ImportMany(AllowRecomposition = true)]
        private IEnumerable<IUpdatable> ImportedUpdatables { get; set; }

        private readonly List<IUpdatable> _updatables = new List<IUpdatable>();
        private readonly List<IInitializable> _initializables = new List<IInitializable>();

        private List<IModule> _modules = new List<IModule>();
        public IEnumerable<IModule> Modules
        {
            get { return _modules; }
        }

        private Application _app;

        public ComponentContainer(Application app)
        {
            _app = app;
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

        public void Update(ApplicationTime time)
        {
            foreach (var updatable in _updatables)
            {
                updatable.Update(time);
            }
        }

        public void OnImportsSatisfied()
        {
            _initializables.AddRange(ImportedInitializables);
            _updatables.AddRange(ImportedUpdatables);

            foreach (var mod in ImportedModules)
            {
                try
                {
                    var module = mod.Value;
                    if (module == null)
                        continue;

                    module.App = _app;

                    _modules.Add(module);
                    _initializables.Add(module);
                    _updatables.Add(module);
                }
                catch (Exception ex)
                {
                    Trace.TraceError("COMPOSITION ERROR: [{0}] {1}", mod.Metadata.Name, ex);
                }
            }
        }
    }
}
