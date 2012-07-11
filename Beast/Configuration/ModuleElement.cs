
using System.Configuration;

namespace Beast.Configuration
{
	public class ModuleElement : ConfigurationElement
	{
		public const string KeyName = "name";
		public const string KeyType = "type";

		[ConfigurationProperty(KeyName, IsRequired = true)]
		public string Name
		{
			get { return (string) base[KeyName]; }
			set { base[KeyName] = value; }
		}

		[ConfigurationProperty(KeyType, IsRequired = true)]
		public string Type
		{
			get { return (string)base[KeyType]; }
			set { base[KeyType] = value; }
		}
	}
}
