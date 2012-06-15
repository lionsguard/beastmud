using System.Windows;
using System.Windows.Controls;
using Beast.Editor.ViewModel;

namespace Beast.Editor
{
	public class DockPaneStyleSelector : StyleSelector
	{
		public Style ToolStyle { get; set; }
		public Style DocumentStyle { get; set; }

		public override Style SelectStyle(object item, DependencyObject container)
		{
			if (item is EditorToolViewModel)
				return ToolStyle;
			if (item is EditorDocumentViewModel)
				return DocumentStyle;
			return base.SelectStyle(item, container);
		}
	}
}
