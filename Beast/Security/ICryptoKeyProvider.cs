
namespace Beast.Security
{
	public interface ICryptoKeyProvider
	{
		string Key { get; }
		string InitializationVector { get; }
		EncryptionAlgorithm Algorithm { get; }
	}
}
