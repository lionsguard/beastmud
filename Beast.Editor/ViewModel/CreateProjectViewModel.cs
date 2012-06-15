using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using Beast.Data;
using Beast.IO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Beast.Editor.ViewModel
{
	public class CreateProjectViewModel : ViewModelBase, IPartImportsSatisfiedNotification
	{
		public ICommand CreateCommand { get; set; }
		public ICommand CancelCommand { get; set; }
		public ICommand BrowseCommand { get; private set; }

		[ImportMany]
		private IEnumerable<IRepository> LoadedRepositories { get; set; }

		public ObservableCollection<Type> Repositories { get; set; }

		#region Project
		public const string ProjectPropertyName = "Project";
		private ProjectViewModel _project = new ProjectViewModel();	
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

				_project = value;

				// Update bindings, no broadcast
				RaisePropertyChanged(ProjectPropertyName);
			}
		}
		#endregion

		#region RepositoryType
		public const string RepositoryTypePropertyName = "RepositoryType";
		private Type _repositoryType;
		public Type RepositoryType
		{	
			get
			{
				return _repositoryType;
			}

			set
			{
				if (_repositoryType == value)
					return;

				_repositoryType = value;
				if (_repositoryType != null)
				{
					_project.Repository = Activator.CreateInstance(_repositoryType) as IRepository;
				}

				// Update bindings, no broadcast
				RaisePropertyChanged(RepositoryTypePropertyName);
			}
		}
		#endregion

		public CreateProjectViewModel()
		{
			BrowseCommand = new RelayCommand(() =>
												{
													var diag = new FolderBrowserDialog
																{
																	ShowNewFolderButton = true
																};
													if (diag.ShowDialog() == DialogResult.OK)
													{
														//RepositoryFilePath = diag.SelectedPath;
														//Project.GameAssembliesPath = diag.SelectedPath;
													}
												});

		}

		#region Implementation of IPartImportsSatisfiedNotification

		public void OnImportsSatisfied()
		{
			Repositories = new ObservableCollection<Type>(LoadedRepositories.Select(r => r.GetType()));
			Repositories.Insert(0, typeof(FileRepository));
		}

		#endregion
	}
}
