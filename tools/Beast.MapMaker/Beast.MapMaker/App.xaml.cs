using Beast.MapMaker.Services;
using Beast.Mapping;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Windows;

namespace Beast.MapMaker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application, IHost, 
        IShutdownService, IMapService, IDialogService, IFileService
    {
        private Beast.Application _beastApp;
        private DefaultMapProvider _mapProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _mapProvider = new DefaultMapProvider();

            SimpleIoc.Default.Register<IShutdownService>(() => this);
            SimpleIoc.Default.Register<IMapService>(() => this);
            SimpleIoc.Default.Register<IDialogService>(() => this);
            SimpleIoc.Default.Register<IFileService>(() => this);

            DependencyResolver.Register<IMapProvider>(() => _mapProvider);

            try
            {
                var settings = ApplicationSettings.FromConfigOrDefault();
                settings.UpdateInterval = TimeSpan.FromHours(1); // Don't need constant updates

                _beastApp = new Application(this, settings);
                _beastApp.Run();

                _mapProvider.Initialize(_beastApp);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
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
        public Map GetMap(string fileName)
        {
            return _mapProvider.GetMapFromFile(fileName);
        }

        public void SaveMap(Map map, string fileName)
        {
            _mapProvider.SaveMapToFile(map, fileName);
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
    }
}
