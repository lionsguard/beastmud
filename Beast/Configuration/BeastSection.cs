using System.Configuration;
using System.Xml;

namespace Beast.Configuration
{
	/// <summary>
	/// Defines a beast configuration section.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Sample Configuration File:
	/// </para>
	/// <para>
	/// <![CDATA[
	/// <beast install="true">
	///	  <modulesDirectory path="modules" isVirtual="true"/>
	///	</beast>
	/// ]]>
	/// </para>
	/// </remarks>
	public class BeastSection : ConfigurationSection
	{
		public const string KeyRoot = "beast";
		public const string KeyModulesDirectory = "modulesDirectory";
		public const string KeyModules = "modules";
		public const string KeyCryptoKeyProviderType = "cryptoKeyProviderType";
		public const string KeyConnectionTimeout = "connectionTimeout";
		public const string KeyGameStepInterval = "gameStepInterval";
		public const string KeyInstall = "install";

		[ConfigurationProperty(KeyModulesDirectory)]
		public ModulesDirectoryElement ModulesDirectory
		{
			get { return (ModulesDirectoryElement)base[KeyModulesDirectory]; }
			set { base[KeyModulesDirectory] = value; }
		}

		[ConfigurationProperty(KeyModules)]
		public ModuleElementCollection Modules
		{
			get { return (ModuleElementCollection)base[KeyModules]; }
			set { base[KeyModules] = value; }
		}

		[ConfigurationProperty(KeyCryptoKeyProviderType)]
		public string CryptoKeyProviderType
		{
			get { return (string)base[KeyCryptoKeyProviderType]; }
			set { base[KeyCryptoKeyProviderType] = value; }
		}

		[ConfigurationProperty(KeyConnectionTimeout)]
		public double ConnectionTimeout
		{
			get { return (double)base[KeyConnectionTimeout]; }
			set { base[KeyConnectionTimeout] = value; }
		}

		[ConfigurationProperty(KeyGameStepInterval)]
		public double GameStepInterval
		{
			get { return (double)base[KeyGameStepInterval]; }
			set { base[KeyGameStepInterval] = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether or not the game installer can be run.
		/// </summary>
		[ConfigurationProperty(KeyInstall, DefaultValue = false)]
		public bool Install
		{
			get { return (bool)base[KeyInstall]; }
			set { base[KeyInstall] = value; }
		}

		public void WriteXml(XmlWriter writer)
		{
			SerializeToXmlElement(writer, KeyRoot);
		}

		public void ReadXml(XmlReader reader)
		{
			DeserializeSection(reader);
		}

		public static BeastSection Load()
		{
			return ConfigurationManager.GetSection(KeyRoot) as BeastSection;
		}

		public static BeastSection Load(string fileName)
		{
			var map = new ExeConfigurationFileMap
			          	{
							ExeConfigFilename = fileName
			          	};
			var config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
			return config.GetSection(KeyRoot) as BeastSection;
		}
	}
}
