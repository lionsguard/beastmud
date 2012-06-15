
using System.Windows.Input;

namespace Beast.Editor.ViewModel
{
	public class ZoneEditorViewModel : EditorViewModel
	{
		public ICommand SaveCommand { get; set; }

		public ZoneEditorViewModel()
		{
			Tools.AddRange(new EditorToolViewModel[]
			               	{
			               		new ZoneListToolViewModel { Title = "Zones" }, 
								new PlacePropertiesToolViewModel { Title = "Properties" }
			               	});
		}
	}

	public class ZoneListToolViewModel : EditorToolViewModel
	{
		public ZoneListToolViewModel()
		{
			
		}
	}

	public class PlacePropertiesToolViewModel : EditorToolViewModel
	{
		
	}

	public class ZoneDocumentViewModel : EditorDocumentViewModel
	{
		
	}
}
