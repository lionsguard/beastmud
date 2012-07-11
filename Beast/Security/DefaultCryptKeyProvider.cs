
namespace Beast.Security
{
	public class DefaultCryptKeyProvider : ICryptoKeyProvider
	{
		public const string DefaultKey = "qVAbAr8f3nBUZbsZ2dsAMg1K5f+6lBql08ewV4/Dd4A=";
		public const string DefaultInitVector = "v3gcRqpVKwazs6gz2tdV7A==";

		public string Key { get; private set; }
		public string InitializationVector { get; private set; }
		public EncryptionAlgorithm Algorithm { get; private set; }

		public DefaultCryptKeyProvider()
		{
			Algorithm = EncryptionAlgorithm.Rijndael;
			Key = DefaultKey;
			InitializationVector = DefaultInitVector;
		}
	}
}
