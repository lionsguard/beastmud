using System;
using System.Collections.Generic;
using Beast.Security;

namespace Beast
{
	public class GameSettings
	{
		public static readonly TimeSpan DefaultGameStepInterval = TimeSpan.FromSeconds(5);
		public static readonly TimeSpan DefaultConnectionTimeout = TimeSpan.FromMinutes(15);

		public string ModulesDirectory { get; set; }
		public IEnumerable<Type> ModuleTypes { get; set; }
		public string FileRepositoryPath { get; set; }
		public ICryptoKeyProvider CryptoKeyProvider { get; set; }
		public TimeSpan GameStepInterval { get; set; }
		public TimeSpan ConnectionTimeout { get; set; }

		public GameSettings()
		{
			GameStepInterval = DefaultGameStepInterval;
			CryptoKeyProvider = new DefaultCryptKeyProvider();
			ConnectionTimeout = DefaultConnectionTimeout;
		}
	}
}