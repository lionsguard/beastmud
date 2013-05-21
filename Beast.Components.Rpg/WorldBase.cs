using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beast.Commands;
using Beast.Serialization;
using Beast.Mapping;
using Beast.Data;
using Beast.Mobiles;

namespace Beast
{
    public abstract class WorldBase : CommandModuleBase, IWorld
    {
        private static readonly object SyncLock = new object();

        private IEnumerable<Terrain> _terrain;
        public IEnumerable<Terrain> Terrain
        {
            get 
            {
                if (_terrain == null)
                {
                    lock (SyncLock)
                    {
                        if (_terrain == null)
                        {
                            _terrain = DependencyResolver.Resolve<IWorldDataProvider>().GetTerrain();
                        }
                    }
                }
                return _terrain;
            }
        }

        private IEnumerable<PlaceFlag> _placeFlags;
        private volatile bool _placeFlagsLoaded;
        public IEnumerable<PlaceFlag> PlaceFlags
        {
            get
            {
                if (!_placeFlagsLoaded)
                {
                    lock (SyncLock)
                    {
                        if (!_placeFlagsLoaded)
                        {
                            _placeFlags = DependencyResolver.Resolve<IWorldDataProvider>().GetPlaceFlags();
                            _placeFlagsLoaded = true;
                        }
                    }
                }
                return _placeFlags;
            }
        }

        public override void Initialize(Application app)
        {
            base.Initialize(app);

            JsonSerialization.AddAssemblies(typeof(WorldBase).Assembly);

            DependencyResolver.Register<IWorld>(() => this);
            DependencyResolver.Register<IMapProvider>(() => CreateAndInitialize<DefaultMapProvider>(app));
            DependencyResolver.Register<IWorldDataProvider>(() => CreateAndInitialize<DefaultWorldDataProvider>(app));

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

        protected T CreateAndInitialize<T>(Application app) where T : IInitializable, new()
        {
            var obj = new T();
            obj.Initialize(app);
            return obj;
        }

        public override void ProcessInput(IConnection connection, IO.IInput input)
        {
            base.ProcessInput(connection, input);

            // Write any output queued with the character.
            var character = connection.Character<IMobile>();
            if (character != null)
            {
                foreach (var output in character.DequeueOutput())
                {
                    connection.Write(output);
                }
            }
        }
    }
}
