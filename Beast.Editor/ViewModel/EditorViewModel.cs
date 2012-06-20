using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Beast.Editor.ViewModel
{
	public abstract class EditorViewModel : ViewModelBase
	{
		public ICommand CloseCommand { get; protected set; }

		public ObservableCollection<EditorToolViewModel> Tools { get; set; }
		public ObservableCollection<EditorDocumentViewModel> Documents { get; set; }

		#region ActiveDocument
		public const string ActiveDocumentPropertyName = "ActiveDocument";
		private EditorItemViewModel _activeDocument;
		public EditorItemViewModel ActiveDocument
		{
			get
			{
				return _activeDocument;
			}

			set
			{
				if (_activeDocument == value)
					return;

				_activeDocument = value;

				// Update bindings, no broadcast
				RaisePropertyChanged(ActiveDocumentPropertyName);
			}
		}
		#endregion

		#region SelectedTool
		public const string SelectedToolPropertyName = "SelectedTool";
		private EditorToolViewModel _selectedTool;	
		public EditorToolViewModel SelectedTool
		{
			get
			{
				return _selectedTool;
			}

			set
			{
				if (_selectedTool != null)
					_selectedTool.IsSelected = false;

				_selectedTool = value;

				if (_selectedTool != null)
					_selectedTool.IsSelected = true;

				// Update bindings, no broadcast
				RaisePropertyChanged(SelectedToolPropertyName);
			}
		}
		#endregion

		protected EditorViewModel()
		{
			Tools = new ObservableCollection<EditorToolViewModel>();
			Documents = new ObservableCollection<EditorDocumentViewModel>();

			CloseCommand = new RelayCommand<EditorDocumentViewModel>(RemoveDocument);
		}

		public void AddAndOpenDocument<T>(T doc) where T : EditorDocumentViewModel
		{
			if (!Documents.Contains(doc))
			{
				doc.CloseCommand = new RelayCommand(()=> CloseCommand.Execute(doc));
				Documents.Add(doc);
			}
		}

		public void RemoveDocument<T>(T doc) where T : EditorDocumentViewModel
		{
			if (doc.IsDirty)
			{
				if (MessageBox.Show(string.Format(Properties.Resources.DocumentSaveDialog_MsgFormat, doc.Title), 
					Properties.Resources.DocumentSaveDialog_Caption, 
					MessageBoxButton.YesNoCancel, 
					MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
				{
					doc.Save();
				}
			}
			Documents.Remove(doc);
		}
	}
}