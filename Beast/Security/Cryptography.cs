using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Beast.Security
{
	/// <summary>
	/// This class is used to perform cryptography operations, namely to decrypt 
	/// the domain, username, and password to impersonate when performing operations.
	/// </summary>
	/// <remarks>
	/// <para>
	/// 
	/// </para>
	/// </remarks>
	public static class Cryptography
	{
		const int SaltSizeMin = 4;
		const int SaltSizeMax = 8;

		private static ICryptoKeyProvider _keyProvider;

		internal static void Initialize(ICryptoKeyProvider keyProvider)
		{
			_keyProvider = keyProvider;
		}

		/// <summary>
		/// Creates a base 64 encoded string from a random salt value.
		/// </summary>
		/// <returns>A base 64 encoded salt string.</returns>
		public static string CreateSalt()
		{
			var crypto = new RNGCryptoServiceProvider();

			var rnd = new Random();
			var size = rnd.Next(SaltSizeMin, SaltSizeMax);

			var data = new byte[size];
			crypto.GetNonZeroBytes(data);

			return Convert.ToBase64String(data);
		}

		/// <summary>
		/// Computes a hash from the specified plainText and salt values.
		/// </summary>
		/// <param name="plainText">The text to hash.</param>
		/// <param name="salt">The salt value used to hash.</param>
		/// <returns>A computed base 64 encoded hashed string.</returns>
		public static string ComputeHash(string plainText, string salt)
		{
			var saltBytes = Convert.FromBase64String(salt);

			var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
			var plainTextAndSaltBytes = new byte[plainTextBytes.Length + saltBytes.Length];

			Array.Copy(plainTextBytes, 0, plainTextAndSaltBytes, 0, plainTextBytes.Length);
			Array.Copy(saltBytes, 0, plainTextAndSaltBytes, plainTextBytes.Length, saltBytes.Length);

			var hash = new SHA1Managed();
			var hashBytes = hash.ComputeHash(plainTextAndSaltBytes);

			return Convert.ToBase64String(hashBytes);
		}

		/// <summary>
		/// Determines the action, either encryption or decrytpion.
		/// </summary>
		private enum Action
		{
			Encrypt,
			Decrypt
		}

		/// <summary>
		/// Generates a key for the specified algorithm.
		/// </summary>
		/// <param name="algorithm">One of the EncryptionAlgorithm values.</param>
		/// <returns>A key for the specified algorithm.</returns>
		public static byte[] GenerateKey(EncryptionAlgorithm algorithm)
		{
			SymmetricAlgorithm crypto = GetCryptoServiceProvider(algorithm);
			crypto.GenerateKey();
			return crypto.Key;
		}

		/// <summary>
		/// Generates an initialization vector for the specified algorithm.
		/// </summary>
		/// <param name="algorithm">One of the EncryptionAlgorithm values.</param>
		/// <returns>An initialization vector for the specified algorithm.</returns>
		public static byte[] GenerateIV(EncryptionAlgorithm algorithm)
		{
			SymmetricAlgorithm crypto = GetCryptoServiceProvider(algorithm);
			crypto.GenerateIV();
			return crypto.IV;
		}

		/// <summary>
		/// Determines if the specified key is a valid size for the specified algorithm.
		/// </summary>
		/// <param name="key">The key string to be evaluated.</param>
		/// <param name="algorithm">The EncryptionAlgorithm to valid the key.</param>
		/// <param name="message">An output value of the valid sizes for the specified algorithm.</param>
		/// <returns>True if the key is a valid size; otherwise false.</returns>
		public static bool IsValidKeySize(string key, EncryptionAlgorithm algorithm, out string message)
		{
			SymmetricAlgorithm crypto = GetCryptoServiceProvider(algorithm);
			if (crypto != null)
			{
				byte[] keyBytes = Encoding.ASCII.GetBytes(key);
				int bitLength = keyBytes.Length * 8;
				StringBuilder sb = new StringBuilder();
				sb.AppendFormat("The size of your key is {0} byte(s).\n", keyBytes.Length);
				foreach (KeySizes sizes in crypto.LegalKeySizes)
				{
					int min = (sizes.MinSize / 8);
					int max = (sizes.MaxSize / 8);
					int skip = (sizes.SkipSize / 8);
					sb.AppendFormat("The minimum key size acceptable is {0} byte(s), the maximum key size " +
						"acceptable is {1} byte(s) and the key must be created in increments of {2} byte(s).\n",
						min, max, skip);
				}
				message = sb.ToString();
				return crypto.ValidKeySize(bitLength);
			}
			message = String.Empty;
			return false;
		}

		/// <summary>
		/// Determines if the specified IV is a valid size for the specified algorithm.
		/// </summary>
		/// <param name="iv">The IV string to be evaluated.</param>
		/// <param name="algorithm">The EncryptionAlgorithm to valid the IV.</param>
		/// <param name="message">An output value of the valid block size for the specified algorithm.</param>
		/// <returns>True if the IV is a valid size; otherwise false.</returns>
		public static bool IsValidBlockSize(string iv, EncryptionAlgorithm algorithm, out string message)
		{
			SymmetricAlgorithm crypto = GetCryptoServiceProvider(algorithm);
			message = String.Empty;
			byte[] ivBytes = null;
			if (crypto != null)
			{
				try
				{
					ivBytes = Encoding.ASCII.GetBytes(iv);
					crypto.IV = ivBytes;
				}
				catch (CryptographicException)
				{
					// Invalid block size.
					StringBuilder sb = new StringBuilder();
					sb.AppendFormat("The size of your initialization vector (IV) is {0} byte(s).\n", ivBytes.Length);
					foreach (KeySizes sizes in crypto.LegalBlockSizes)
					{
						int min = (sizes.MinSize / 8);
						int max = (sizes.MaxSize / 8);
						int skip = (sizes.SkipSize / 8);
						sb.AppendFormat("The minimum IV size acceptable is {0} byte(s), the maximum IV size " +
							"acceptable is {1} byte(s) and the IV must be created in increments of {2} byte(s).\n",
							min, max, skip);
					}
					message = sb.ToString();
					return false;
				}
				return true;
			}
			return false;
		}

		/// <summary>
		/// Gets an instance of the SymmetricAlgorithm class for the specified algorithm.
		/// </summary>
		/// <param name="algorithm">The EncryptionAlgorithm value specifying the type of crypto service provider to create.</param>
		/// <returns>A SymmetricAlgorithm instance for the specified algorithm preset with the specified Key and IV.</returns>
		private static SymmetricAlgorithm GetCryptoServiceProvider(EncryptionAlgorithm algorithm)
		{
			SymmetricAlgorithm crypto = null;
			switch (algorithm)
			{
				case EncryptionAlgorithm.Des:
					crypto = new DESCryptoServiceProvider();
					break; 
				case EncryptionAlgorithm.Rc2:
					crypto = new RC2CryptoServiceProvider();
					break;
				case EncryptionAlgorithm.Rijndael:
					crypto = new RijndaelManaged();
					break;
				case EncryptionAlgorithm.TripleDes:
					crypto = new TripleDESCryptoServiceProvider();
					break;
				default:
					throw new NotSupportedException(String.Concat("The EncryptionAlgorithm value specified '", 
						algorithm.ToString(), "' is not supported."));
			}
			return crypto;
		}

		/// <summary>
		/// Gets an instance of the SymmetricAlgorithm class for the specified algorithm.
		/// </summary>
		/// <param name="key">Byte array representing the encryption key.</param>
		/// <param name="iv">The intialization vector for the crypto service provider.</param>
		/// <param name="algorithm">The EncryptionAlgorithm value specifying the type of crypto service provider to create.</param>
		/// <returns>A SymmetricAlgorithm instance for the specified algorithm preset with the specified Key and IV.</returns>
		private static SymmetricAlgorithm GetCryptoServiceProvider(byte[] key, byte[] iv, EncryptionAlgorithm algorithm)
		{
			SymmetricAlgorithm crypto = GetCryptoServiceProvider(algorithm);
			if (crypto != null)
			{
				crypto.Mode = CipherMode.CBC;
				crypto.Key = key;
				crypto.IV = iv;
			}
			return crypto;
		}

		/// <summary>
		/// Creates an ICryptoTransform instance for the specified algorithm and action.
		/// </summary>
		/// <param name="key">Byte array representing the encryption key.</param>
		/// <param name="iv">The intialization vector for the crypto service provider.</param>
		/// <param name="algorithm">The EncryptionAlgorithm value specifying the type of crypto service provider to create.</param>
		/// <param name="action">The Action value which will specifiy the type of ICryptoTransform to return.</param>
		/// <returns>An ICryptoTransform instance for the specified algorithm and action.</returns>
		private static ICryptoTransform GetCryptoTransform(
			byte[] key, 
			byte[] iv, 
			EncryptionAlgorithm algorithm, 
			Action action)
		{
			switch (action)
			{
				case Action.Decrypt: return GetCryptoServiceProvider(key, iv, algorithm).CreateDecryptor();
				case Action.Encrypt: return GetCryptoServiceProvider(key, iv, algorithm).CreateEncryptor();
			}
			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="plainText">The plain text to encrypt.</param>
		/// <param name="encryptKey">The unencrypted master kye to use when encrypting plainText.</param>
		/// <param name="encryptIV">The unencrypted initialization vector used to encrypt plainText.</param>
		/// <param name="algorithm">The EncryptionAlgorithm value specifying the type of crypto service provider to create.</param>
		/// <param name="action">The Action value which will specifiy the type of ICryptoTransform to return.</param>
		/// <returns></returns>
		private static byte[] GetCipher(
			string plainText, 
			string encryptKey, 
			string encryptIV, 
			EncryptionAlgorithm algorithm, 
			Action action)
		{
			// Set up a memory stream to hold the encrypted data.
			MemoryStream memStream = new MemoryStream();

			byte[] data = null;
			byte[] key = Convert.FromBase64String(encryptKey);
			byte[] iv = Convert.FromBase64String(encryptIV);
			byte[] cipher = null;

			switch (action)
			{
				case Action.Decrypt: 
					data = Convert.FromBase64String(plainText); 
					break;
				case Action.Encrypt:
					data = Encoding.ASCII.GetBytes(plainText);
					break;
			}

			ICryptoTransform xForm = GetCryptoTransform(key, iv, algorithm, action);
			CryptoStream encStream = new CryptoStream(memStream, xForm, CryptoStreamMode.Write);
			try
			{
				// Encrypt the data and write it to the memory stream.
				encStream.Write(data, 0, data.Length);
				encStream.FlushFinalBlock();
				cipher = memStream.ToArray();
			}
			catch (Exception ex)
			{
				throw (ex);
			}
			finally
			{	
				encStream.Close();
				memStream.Close();
			}

			// Return the encrypted data.
			return cipher;
		}

		/// <summary>
		/// Encrypts the specified plainText using encryptKey as the key in the Crypto Service Provider 
		/// specified by algorithm.
		/// </summary>
		/// <param name="plainText">The plain text to encrypt.</param>
		/// <returns>An encrypted string using the encryption algorithm specified.</returns>
		public static string Encrypt(string plainText)
		{
			return Encrypt(plainText, _keyProvider.Key, _keyProvider.InitializationVector, _keyProvider.Algorithm);
		}

		/// <summary>
		/// Encrypts the specified plainText using encryptKey as the key in the Crypto Service Provider 
		/// specified by algorithm.
		/// </summary>
		/// <param name="plainText">The plain text to encrypt.</param>
		/// <param name="encryptKey">The unencrypted master kye to use when encrypting plainText.</param>
		/// <param name="encryptIV">The initialization vector used to encrypt plainText.</param>
		/// <param name="algorithm">The encryption algorithm to use to encrypt plainText.</param>
		/// <returns>An encrypted string using the encryption algorithm specified.</returns>
		public static string Encrypt(
			string plainText, 
			string encryptKey, 
			string encryptIV, 
			EncryptionAlgorithm algorithm)
		{
			byte[] cipher = GetCipher(plainText, encryptKey, encryptIV, algorithm, Action.Encrypt);

			// Return the encrypted data.
			if (cipher != null)
			{
				return Convert.ToBase64String(cipher);
			}
			return String.Empty;
		} // End Encrypt

		/// <summary>
		/// Decrypts the specified plainText using encryptKey as the key in the Crypto Service Provider 
		/// specified by algorithm.
		/// </summary>
		/// <param name="cipherText">The cipher text to decrypt.</param>
		/// <returns>A decrypted string using the decryption algorithm specified.</returns>
		public static string Decrypt(string cipherText)
		{
			return Decrypt(cipherText, _keyProvider.Key, _keyProvider.InitializationVector, _keyProvider.Algorithm);
		}

		/// <summary>
		/// Decrypts the specified plainText using encryptKey as the key in the Crypto Service Provider 
		/// specified by algorithm.
		/// </summary>
		/// <param name="cipherText">The cipher text to decrypt.</param>
		/// <param name="encryptKey">The unencrypted master kye to use when encrypting plainText.</param>
		/// <param name="encryptIV">The initialization vector used to encrypt plainText.</param>
		/// <param name="algorithm">The encryption algorithm to use to encrypt plainText.</param>
		/// <returns>A decrypted string using the decryption algorithm specified.</returns>
		public static string Decrypt(
			string cipherText, 
			string encryptKey, 
			string encryptIV, 
			EncryptionAlgorithm algorithm)
		{
			byte[] cipher = GetCipher(cipherText, encryptKey, encryptIV, algorithm, Action.Decrypt);

			// Return the encrypted data.
			if (cipher != null)
			{
				return Encoding.ASCII.GetString(cipher);
			}
			return String.Empty;
		} // End Decrypt


		/// <summary>
		/// Generates a hash for the given plain text value and returns a
		/// base64-encoded result. Before the hash is computed, a random salt
		/// is generated and appended to the plain text. This salt is stored at
		/// the end of the hash value, so it can be used later for hash
		/// verification.
		/// </summary>
		/// <param name="plainText">
		/// Plaintext value to be hashed. The function does not check whether
		/// this parameter is null.
		/// </param>
		/// <param name="hashAlgorithm">
		/// Name of the hash algorithm. Allowed values are: "MD5", "SHA1",
		/// "SHA256", "SHA384", and "SHA512" (if any other value is specified
		/// MD5 hashing algorithm will be used). This value is case-insensitive.
		/// </param>
		/// <param name="saltBytes">
		/// Salt bytes. This parameter can be null, in which case a random salt
		/// value will be generated.
		/// </param>
		/// <returns>
		/// Hash value formatted as a base64-encoded string.
		/// </returns>
		public static string ComputeHash(string plainText,
										 string hashAlgorithm,
										 byte[] saltBytes)
		{
			// If salt is not specified, generate it on the fly.
			if (saltBytes == null)
			{
				// Define min and max salt sizes.
				int minSaltSize = 4;
				int maxSaltSize = 8;

				// Generate a random number for the size of the salt.
				Random random = new Random();
				int saltSize = random.Next(minSaltSize, maxSaltSize);

				// Allocate a byte array, which will hold the salt.
				saltBytes = new byte[saltSize];

				// Initialize a random number generator.
				RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

				// Fill the salt with cryptographically strong byte values.
				rng.GetNonZeroBytes(saltBytes);
			}

			// Convert plain text into a byte array.
			byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

			// Allocate array, which will hold plain text and salt.
			byte[] plainTextWithSaltBytes =
					new byte[plainTextBytes.Length + saltBytes.Length];

			// Copy plain text bytes into resulting array.
			for (int i = 0; i < plainTextBytes.Length; i++)
				plainTextWithSaltBytes[i] = plainTextBytes[i];

			// Append salt bytes to the resulting array.
			for (int i = 0; i < saltBytes.Length; i++)
				plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

			// Because we support multiple hashing algorithms, we must define
			// hash object as a common (abstract) base class. We will specify the
			// actual hashing algorithm class later during object creation.
			HashAlgorithm hash;

			// Make sure hashing algorithm name is specified.
			if (hashAlgorithm == null)
				hashAlgorithm = "";

			// Initialize appropriate hashing algorithm class.
			switch (hashAlgorithm.ToUpper())
			{
				case "SHA1":
					hash = new SHA1Managed();
					break;

				case "SHA256":
					hash = new SHA256Managed();
					break;

				case "SHA384":
					hash = new SHA384Managed();
					break;

				case "SHA512":
					hash = new SHA512Managed();
					break;

				default:
					hash = new MD5CryptoServiceProvider();
					break;
			}

			// Compute hash value of our plain text with appended salt.
			byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

			// Create array which will hold hash and original salt bytes.
			byte[] hashWithSaltBytes = new byte[hashBytes.Length +
												saltBytes.Length];

			// Copy hash bytes into resulting array.
			for (int i = 0; i < hashBytes.Length; i++)
				hashWithSaltBytes[i] = hashBytes[i];

			// Append salt bytes to the result.
			for (int i = 0; i < saltBytes.Length; i++)
				hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

			// Convert result into a base64-encoded string.
			string hashValue = Convert.ToBase64String(hashWithSaltBytes);

			// Return the result.
			return hashValue;
		}

		/// <summary>
		/// Compares a hash of the specified plain text value to a given hash
		/// value. Plain text is hashed with the same salt value as the original
		/// hash.
		/// </summary>
		/// <param name="plainText">
		/// Plain text to be verified against the specified hash. The function
		/// does not check whether this parameter is null.
		/// </param>
		/// <param name="hashAlgorithm">
		/// Name of the hash algorithm. Allowed values are: "MD5", "SHA1", 
		/// "SHA256", "SHA384", and "SHA512" (if any other value is specified,
		/// MD5 hashing algorithm will be used). This value is case-insensitive.
		/// </param>
		/// <param name="hashValue">
		/// Base64-encoded hash value produced by ComputeHash function. This value
		/// includes the original salt appended to it.
		/// </param>
		/// <returns>
		/// If computed hash mathes the specified hash the function the return
		/// value is true; otherwise, the function returns false.
		/// </returns>
		public static bool VerifyHash(string plainText,
									  string hashAlgorithm,
									  string hashValue)
		{
			// Convert base64-encoded hash value into a byte array.
			byte[] hashWithSaltBytes = Convert.FromBase64String(hashValue);

			// We must know size of hash (without salt).
			int hashSizeInBits, hashSizeInBytes;

			// Make sure that hashing algorithm name is specified.
			if (hashAlgorithm == null)
				hashAlgorithm = "";

			// Size of hash is based on the specified algorithm.
			switch (hashAlgorithm.ToUpper())
			{
				case "SHA1":
					hashSizeInBits = 160;
					break;

				case "SHA256":
					hashSizeInBits = 256;
					break;

				case "SHA384":
					hashSizeInBits = 384;
					break;

				case "SHA512":
					hashSizeInBits = 512;
					break;

				default: // Must be MD5
					hashSizeInBits = 128;
					break;
			}

			// Convert size of hash from bits to bytes.
			hashSizeInBytes = hashSizeInBits / 8;

			// Make sure that the specified hash value is long enough.
			if (hashWithSaltBytes.Length < hashSizeInBytes)
				return false;

			// Allocate array to hold original salt bytes retrieved from hash.
			byte[] saltBytes = new byte[hashWithSaltBytes.Length -
										hashSizeInBytes];

			// Copy salt from the end of the hash to the new array.
			for (int i = 0; i < saltBytes.Length; i++)
				saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];

			// Compute a new hash string.
			string expectedHashString =
						ComputeHash(plainText, hashAlgorithm, saltBytes);

			// If the computed hash matches the specified hash,
			// the plain text value must be correct.
			return (hashValue == expectedHashString);
		}
	}
}
