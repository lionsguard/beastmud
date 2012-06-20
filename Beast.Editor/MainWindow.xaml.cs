using System.Windows;
using System.Windows.Threading;
using Beast.Editor.ViewModel;

namespace Beast.Editor
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			Closing += (s, e) => ViewModelLocator.Cleanup();
			Application.Current.DispatcherUnhandledException += OnDispatcherUnhandledException;
		}

		private static void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine(e.Exception);
			e.Handled = true;
		}
	}
}
