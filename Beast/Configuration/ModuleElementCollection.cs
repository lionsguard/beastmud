using System.Configuration;

namespace Beast.Configuration
{
	public class ModuleElementCollection : ConfigurationElementCollection
	{
		#region Overrides of ConfigurationElementCollection

		protected override ConfigurationElement CreateNewElement()
		{
			return new ModuleElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((ModuleElement) element).Name;
		}

		#endregion
	}
}