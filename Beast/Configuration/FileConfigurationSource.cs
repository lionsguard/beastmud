using System.Configuration;
using Enumerable = System.Linq.Enumerable;

namespace Beast.Configuration
{
	public class FileConfigurationSource : IConfigurationSource
	{
		#region Implementation of IConfigurationSource

		public bool Contains(string key)
		{
			return Enumerable.Contains(ConfigurationManager.AppSettings.AllKeys, key);
		}

		public string GetValue(string key)
		{
			return ConfigurationManager.AppSettings[key];
		}

		#endregion
	}
}