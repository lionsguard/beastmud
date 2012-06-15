using System;
using GalaSoft.MvvmLight;

namespace Beast.Editor.ViewModel
{
	public class EditorItemViewModel : ViewModelBase
	{
		#region Title
		public const string TitlePropertyName = "Title";
		private string _title;	
		public string Title
		{
			get
			{
				return _title;
			}

			set
			{
				if (_title == value)
					return;

				_title = value;

				// Update bindings, no broadcast
				RaisePropertyChanged(TitlePropertyName);
			}
		}
		#endregion

		#region IconSource
		public const string IconSourcePropertyName = "IconSource";
		private Uri _iconSource;	
		public Uri IconSource
		{
			get
			{
				return _iconSource;
			}

			set
			{
				if (_iconSource == value)
					return;

				_iconSource = value;

				// Update bindings, no broadcast
				RaisePropertyChanged(IconSourcePropertyName);
			}
		}
		#endregion

		#region ContentId
		public const string ContentIdPropertyName = "ContentId";
		private string _contentId;	
		public string ContentId
		{
			get
			{
				return _contentId;
			}

			set
			{
				if (_contentId == value)
					return;

				_contentId = value;

				// Update bindings, no broadcast
				RaisePropertyChanged(ContentIdPropertyName);
			}
		}
		#endregion
	}
}