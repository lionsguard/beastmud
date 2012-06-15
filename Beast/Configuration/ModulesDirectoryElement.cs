using System.Configuration;

namespace Beast.Configuration
{
	public class ModulesDirectoryElement : ConfigurationElement
	{
		public const string KeyPath = "path";
		public const string KeyIsVirtual = "isVirtual";

		[ConfigurationProperty(KeyPath, IsRequired = true)]
		public string Path
		{
			get { return (string)base[KeyPath]; }
			set { base[KeyPath] = value; }
		}

		[ConfigurationProperty(KeyIsVirtual, DefaultValue = false)]
		public bool IsVirtual
		{
			get { return (bool)base[KeyIsVirtual]; }
			set { base[KeyIsVirtual] = value; }
		}
	}
}