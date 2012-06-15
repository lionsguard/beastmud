using System;
using System.Configuration;
using Beast.Data;

namespace Beast.Configuration
{
	public class RepositoryElement : ConfigurationElement
	{
		public const string KeyType = "type";

		public new object this[ConfigurationProperty key]
		{
			get { return base[key]; }
			set
			{
				if (!Properties.Contains(key.Name))
					Properties.Add(key);
				base[key] = value;
			}
		}

		[ConfigurationProperty(KeyType, IsRequired = true)]
		public string Type
		{
			get { return (string) base[KeyType]; }
			set { base[KeyType] = value; }
		}

		protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
		{
			var prop = new ConfigurationProperty(name, typeof (string));
			Properties.Add(prop);
			base[prop] = value;
			return true;
		}

		public IRepository ToRepository()
		{
			var type = System.Type.GetType(Type, false, true);
			if (type == null)
				return null;
			var repo = Activator.CreateInstance(type) as IRepository;
			if (repo != null)
			{
				repo.FromConfig(this);
			}
			return repo;
		}
	}
}