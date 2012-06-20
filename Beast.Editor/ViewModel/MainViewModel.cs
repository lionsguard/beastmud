using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Beast.Commands;
using Beast.Editor.Dialogs;
using Beast.Editor.ViewModel.Zones;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace Beast.Editor.ViewModel
{
	/// <summary>
	/// This class contains properties that the main View can data bind to.
	/// <para>
	/// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
	/// </para>
	/// <para>
	/// You can also use Blend to data bind with the tool's support.
	/// </para>
	/// <para>
	/// See http://www.galasoft.ch/mvvm/getstarted
	/// </para>
	/// </summary>
	public class MainViewModel : ViewModelBase
	{
		public const string FileExtension = "beast";
		public const string CommandDefsDirectory = "CommandDefinitions";

		public ICommand AboutCommand { get; private set; }
		public ICommand ExitCommand { get; private set; }
		public ICommand CreateProjectCommand { get; private set; }
		public ICommand OpenProjectCommand { get; private set; }

		#region Project
		public const string ProjectPropertyName = "Project";
		private ProjectViewModel _project;	
		public ProjectViewModel Project
		{
			get
			{
				return _project;
			}

			set
			{
				if (_project == value)
					return;

				if (_project != null)
					_project.PropertyChanged -= OnProjectChanged;

				_project = value;

				if (_project != null)
					_project.PropertyChanged += OnProjectChanged;

				// Update bindings, no broadcast
				RaisePropertyChanged(ProjectPropertyName);

				UpdateTitle();
			}
		}
		private void OnProjectChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			UpdateTitle();
		}
		#endregion

		#region Title
		public const string TitlePropertyName = "Title";
		public string Title
		{
			get
			{
				if (Project == null)
					return Properties.Resources.BaseTitle;

				return string.Format(Properties.Resources.TitleFormat, Properties.Resources.BaseTitle, Project.Name, Project.IsDirty ? Properties.Resources.FileDirtyIndicator : string.Empty);
			}
		}
		private void UpdateTitle()
		{
			RaisePropertyChanged(TitlePropertyName);
		}
		#endregion

		#region CreateProjectModel
		public const string CreateProjectModelPropertyName = "CreateProjectModel";
		private CreateProjectViewModel _createProjectModel;
		public CreateProjectViewModel CreateProjectModel
		{
			get	
			{
				return _createProjectModel;
			}

			set
			{
				if (_createProjectModel == value)
					return;

				_createProjectModel = value;

				// Update bindings, no broadcast
				RaisePropertyChanged(CreateProjectModelPropertyName);
			}
		}
		#endregion

		#region CurrentEditor
		public const string CurrentEditorPropertyName = "CurrentEditor";
		private EditorViewModel _currentEditor;	
		public EditorViewModel CurrentEditor
		{
			get
			{
				return _currentEditor;
			}

			set
			{
				if (_currentEditor == value)
					return;

				_currentEditor = value;

				// Update bindings, no broadcast
				RaisePropertyChanged(CurrentEditorPropertyName);
			}
		}
		#endregion

		/// <summary>
		/// Initializes a new instance of the MainViewModel class.
		/// </summary>
		public MainViewModel()
		{
			AboutCommand = new RelayCommand<Window>(w =>
			                                        	{
			                                        		var abt = new About(w);
			                                        		abt.ShowDialog();
			                                        	});
			ExitCommand = new RelayCommand(() => Application.Current.Shutdown());

			CreateProjectCommand = new RelayCommand(() =>
			                                        	{
			                                        		var diag = new CreateProjectDialog
			                                        		           	{
			                                        		           		DataContext = CreateProjectModel
			                                        		           	};
			                                        		diag.ShowDialog();
			                                        	});
			OpenProjectCommand = new RelayCommand(HandleOpenProjectCommand);

			CreateProjectModel = new CreateProjectViewModel
								{
									CreateCommand = new RelayCommand<Window>(w =>
																		{
																			w.Close();
																			Project = CreateProjectModel.Project;
																			SaveProject();
																			OnNewProjectCreated();
																		}),
									CancelCommand = new RelayCommand<Window>(w => w.Close())
								};

			if (!IsInDesignMode)
			{
				var catalog = new AggregateCatalog();
				catalog.Catalogs.Add(new AssemblyCatalog(typeof(MainViewModel).Assembly));

				var pluginsPath = Path.Combine(Environment.CurrentDirectory, Properties.Resources.PluginsDirectory);
				if (!Directory.Exists(pluginsPath))
					Directory.CreateDirectory(pluginsPath);
				catalog.Catalogs.Add(new DirectoryCatalog(pluginsPath));

				var container = new CompositionContainer(catalog);
				container.ComposeParts(this, CreateProjectModel);
			}
		}

		private static string GetSaveDirectory()
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), Properties.Resources.RootFolder);
		}

		public void SaveProject()
		{
			if (Project == null)
			{
				MessageBox.Show(Properties.Resources.SaveProjectDialog_ProjectEmpty_Msg, Properties.Resources.SaveProjectDialog_ProjectEmpty_Caption, MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			var path = GetSaveDirectory();
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			if (string.IsNullOrEmpty(Project.FileName))
			{
				var diag = new SaveFileDialog
				{
					AddExtension = true,
					DefaultExt = FileExtension,
					Filter = Properties.Resources.FileFilter,
					InitialDirectory = path
				};
				if (diag.ShowDialog() == true)
				{
					Project.FileName = diag.FileName;
				}
			}

			if (string.IsNullOrEmpty(Project.FileName))
				return;

			using (var fs = File.Create(Project.FileName))
			{
				Project.Save(fs);
			}
			UpdateTitle();
		}

		private void HandleOpenProjectCommand()
		{
			var diag = new OpenFileDialog
			           	{
							DefaultExt = FileExtension,
							Filter = Properties.Resources.FileFilter,
							InitialDirectory = GetSaveDirectory(),
							Title = Properties.Resources.OpenProjectDialog_Title
			           	};

			if (diag.ShowDialog() == true)
			{
				using (var fs = File.OpenRead(diag.FileName))
				{
					Project = ProjectViewModel.Load(fs);
					Project.FileName = diag.FileName;
				}

				// Start the game engine.
				Game.Current.Start(Project.Config);

				CurrentEditor = new ZoneEditorViewModel();
			}
		}

		private void OnNewProjectCreated()
		{
			// Initialize Repo
			Project.Repository.Initialize();

			// Load default/initial data
			AddCommandsToRepo();
		}

		private void AddCommandsToRepo()
		{
			var path = Path.Combine(Directory.GetCurrentDirectory(), CommandDefsDirectory);
			foreach (var file in Directory.GetFiles(path, "*.txt"))
			{
				Project.Repository.SaveCommandDefinition(JsonConvert.DeserializeObject<CommandDefinition>(File.ReadAllText(file)));
			}
		}

		public override void Cleanup()
		{
			if (Game.Current.IsRunning)
			{
				Game.Current.Stop();
			}
			base.Cleanup();
		}
	}
}