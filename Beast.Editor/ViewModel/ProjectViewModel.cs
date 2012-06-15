using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Beast.Configuration;
using Beast.Data;
using GalaSoft.MvvmLight;

namespace Beast.Editor.ViewModel
{
	[XmlRoot(XmlRootName)]
	public class ProjectViewModel : ViewModelBase, IXmlSerializable
	{
		public const string XmlRootName = "beastMudProject";
		public const string XmlNodeName = "name";
		public const string XmlNodeConfig = "configuration";

		public bool IsDirty { get; private set; }
		public string FileName { get; set; }
		public BeastSection Config { get; private set; }

		#region Name
		public const string NamePropertyName = "Name";
		private string _name;	
		public string Name
		{
			get
			{
				return _name;
			}

			set
			{
				if (_name == value)
					return;

				_name = value;

				// Update bindings, no broadcast
				RaisePropertyChanged(NamePropertyName);
			}
		}
		#endregion

		#region Repository
		public const string RepositoryPropertyName = "Repository";
		private IRepository _repository;
		public IRepository Repository
		{
			get
			{
				return _repository;
			}

			set
			{
				if (_repository == value)
					return;

				_repository = value;

				// Update bindings, no broadcast
				RaisePropertyChanged(RepositoryPropertyName);
			}
		}
		#endregion

		protected override void RaisePropertyChanged(string propertyName)
		{
			IsDirty = true;
			base.RaisePropertyChanged(propertyName);
		}

		private BeastSection GetConfig()
		{
			var section = new BeastSection();
			if (Repository != null)
				section.Repository = Repository.ToConfig();
			return section;
		}

		public void Save(Stream stream)
		{
			var serializer = new XmlSerializer(typeof (ProjectViewModel));
			serializer.Serialize(stream, this);

			IsDirty = false;
		}

		public static ProjectViewModel Load(Stream stream)
		{
			var serializer = new XmlSerializer(typeof(ProjectViewModel));
			var model = serializer.Deserialize(stream) as ProjectViewModel;
			if (model != null)
				model.IsDirty = false;
			return model;
		}

		#region Implementation of IXmlSerializable

		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			while (reader.Read())
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					if (reader.Name == XmlNodeName)
					{
						Name = reader.ReadString();	
					}
					if (reader.Name == XmlNodeConfig)
					{
						Config = GetConfig();
						Config.ReadXml(reader);
						Repository = Config.Repository.ToRepository();
					}
				}
			}
		}

		public void WriteXml(XmlWriter writer)
		{
			// Name
			writer.WriteStartElement(XmlNodeName);
			writer.WriteString(Name);
			writer.WriteEndElement();

			// Config
			writer.WriteStartElement(XmlNodeConfig);
			Config = GetConfig();
			Config.WriteXml(writer);
			writer.WriteEndElement();
		}

		#endregion
	}
}
