using Beast.Data;
using Beast.MapMaker.Services;
using Beast.MapMaker.ViewModel;
using Beast.Mapping;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace Beast.MapMaker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application, IHost, 
        IShutdownService, IMapService, IDialogService, IFileService, IWorldDataService
    {
        private Beast.Application _beastApp;
        private IWorld _world;
        private DefaultMapProvider _mapProvider;
        private DefaultWorldDataProvider _worldDataProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            _mapProvider = new DefaultMapProvider();
            _worldDataProvider = new DefaultWorldDataProvider();

            SimpleIoc.Default.Register<IShutdownService>(() => this);
            SimpleIoc.Default.Register<IDialogService>(() => this);
            SimpleIoc.Default.Register<IFileService>(() => this);

            try
            {
                var settings = ApplicationSettings.FromConfigOrDefault();
                settings.UpdateInterval = TimeSpan.FromHours(1); // Don't need constant updates

                _beastApp = new Application(this, settings);
                _beastApp.Run();

                EnsureDependencies();

                _mapProvider.Initialize(_beastApp);
                _worldDataProvider.Initialize(_beastApp);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            base.OnStartup(e);
        }

        private void EnsureDependencies()
        {
            _world = DependencyResolver.Resolve<IWorld>();
            if (_world == null)
            {
                _world = new MapperWorld();
                _world.Initialize(_beastApp);
                DependencyResolver.Register<IWorld>(() => _world);
            }

            if (!SimpleIoc.Default.IsRegistered<IMapService>())
            {
                SimpleIoc.Default.Register<IMapService>(() => this);
                DependencyResolver.Register<IMapProvider>(() => _mapProvider);
            }

            if (!SimpleIoc.Default.IsRegistered<IWorldDataService>())
            {
                SimpleIoc.Default.Register<IWorldDataService>(() => this);
                DependencyResolver.Register<IWorldDataProvider>(() => _worldDataProvider);
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (_beastApp != null)
            {
                _beastApp.Dispose();
            }
            base.OnExit(e);
        }

        #region MapService
        private string _mapFileName;
        public Map GetMap()
        {
            var file = SimpleIoc.Default.GetInstance<IFileService>().OpenFile("Open Map", "Map files|*.json");
            if (!string.IsNullOrEmpty(file))
            {
                _mapFileName = file;
                return _mapProvider.GetMapFromFile(_mapFileName);
            }
            return null;
        }

        public void SaveMap(Map map)
        {
            if (string.IsNullOrEmpty(_mapFileName))
            {
                _mapFileName = SimpleIoc.Default.GetInstance<IFileService>().SaveFile("Save Map", "Map files|*.json", "json");
            }

            if (string.IsNullOrEmpty(_mapFileName))
                return;

            _mapProvider.SaveMapToFile(map, _mapFileName);
        }
        #endregion

        #region DialogService
        public bool? ShowDialog<T>() where T : Window
        {
            var win = Activator.CreateInstance<T>();
            return win.ShowDialog();
        }

        public bool? ShowDialog<T>(object dataContext) where T : Window
        {
            var win = Activator.CreateInstance<T>();
            win.DataContext = dataContext;
            return win.ShowDialog();
        }

        public void PerformWaitOperation(Action callback)
        {
            //var wait = new Wait();
            //wait.Show();
            //if (callback != null)
            //    callback();
            //wait.Close();
        }

        public MessageBoxResult ShowMessageBox(string text, string caption, MessageBoxButton button, MessageBoxImage image)
        {
            return MessageBox.Show(text, caption, button, image);
        }
        #endregion

        #region FileService
        public string OpenFile(string windowTitle, string filter)
        {
            var fd = new OpenFileDialog
            {
                Filter = filter,
                Title = windowTitle
            };
            if (fd.ShowDialog() == true)
                return fd.FileName;
            return null;
        }
        public string SaveFile(string windowTitle, string filter, string extension)
        {
            var fd = new SaveFileDialog
            {
                Filter = filter,
                Title = windowTitle,
                DefaultExt = extension
            };
            if (fd.ShowDialog() == true)
                return fd.FileName;
            return null;
        }
        #endregion

        #region World Data
        private string _terrainFileName;
        private string _placeFlagsFileName;
        public IEnumerable<Terrain> GetTerrain()
        {
            var file = SimpleIoc.Default.GetInstance<IFileService>().OpenFile("Open Terrain File", "JSON files|*.json");
            if (!string.IsNullOrEmpty(file))
            {
                _terrainFileName = file;
                return _worldDataProvider.GetTerrain(_terrainFileName);
            }
            return Terrain.DefaultTerrain;
        }

        public void SaveTerrain(IEnumerable<Terrain> terrain)
        {
            if (string.IsNullOrEmpty(_terrainFileName))
            {
                _terrainFileName = SimpleIoc.Default.GetInstance<IFileService>().SaveFile("Save Terrain", "JSON files|*.json", "json");
            }

            if (string.IsNullOrEmpty(_terrainFileName))
                return;

            _worldDataProvider.SaveTerrain(terrain, _terrainFileName);
        }

        public IEnumerable<PlaceFlag> GetPlaceFlags()
        {
            var file = SimpleIoc.Default.GetInstance<IFileService>().OpenFile("Open Place Flags File", "JSON files|*.json");
            if (!string.IsNullOrEmpty(file))
            {
                _placeFlagsFileName = file;
                return _worldDataProvider.GetPlaceFlags(_placeFlagsFileName);
            }
            return PlaceFlag.All;
        }

        public void SavePlaceFlags(IEnumerable<PlaceFlag> flags)
        {
            if (string.IsNullOrEmpty(_placeFlagsFileName))
            {
                _placeFlagsFileName = SimpleIoc.Default.GetInstance<IFileService>().SaveFile("Save Place Flags", "JSON files|*.json", "json");
            }

            if (string.IsNullOrEmpty(_placeFlagsFileName))
                return;

            _worldDataProvider.SavePlaceFlags(flags, _placeFlagsFileName);
        }
        #endregion
    }
}
