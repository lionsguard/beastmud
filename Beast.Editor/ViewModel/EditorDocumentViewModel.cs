using System.Windows.Input;

namespace Beast.Editor.ViewModel
{
	public class EditorDocumentViewModel : EditorItemViewModel
	{
		public ICommand CloseCommand { get; set; }
		public bool IsDirty { get; protected set; }

		public virtual void Save()
		{
			
		}
	}
}