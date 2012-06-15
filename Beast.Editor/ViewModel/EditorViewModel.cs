using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace Beast.Editor.ViewModel
{
	public abstract class EditorViewModel : ViewModelBase
	{
		public ObservableCollection<EditorToolViewModel> Tools { get; set; }
		public ObservableCollection<EditorDocumentViewModel> Documents { get; set; }

		#region ActiveDocument
		public const string ActiveDocumentPropertyName = "ActiveDocument";
		private EditorDocumentViewModel _activeDocument;	
		public EditorDocumentViewModel ActiveDocument
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

		protected EditorViewModel()
		{
			Tools = new ObservableCollection<EditorToolViewModel>();
			Documents = new ObservableCollection<EditorDocumentViewModel>();
		}
	}
}