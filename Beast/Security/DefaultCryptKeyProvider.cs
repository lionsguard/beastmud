using System;

namespace Beast.Security
{
	public class DefaultCryptKeyProvider : ICryptoKeyProvider
	{
		public string Key { get; private set; }
		public string InitializationVector { get; private set; }
		public EncryptionAlgorithm Algorithm { get; private set; }

		public DefaultCryptKeyProvider()
		{
			Algorithm = EncryptionAlgorithm.Rijndael;
			Key = Convert.ToBase64String(Cryptography.GenerateKey(Algorithm));
			InitializationVector = Convert.ToBase64String(Cryptography.GenerateIV(Algorithm));
		}
	}
}
