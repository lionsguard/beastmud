using System;
using System.Security.Cryptography;
using System.Text;

namespace Beast.Security
{
    /// <summary>
    /// Represents a set of methods for computing cryptographic hashes and data.
    /// </summary>
    public static class Cryptography
    {
        const int SaltSizeMin = 4;
        const int SaltSizeMax = 8;

        /// <summary>
        /// Creates a salt string for hashing.
        /// </summary>
        /// <returns>The Base64 representation of the salt bytes.</returns>
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
        /// Computes a hash using the specified plainText and salt.
        /// </summary>
        /// <param name="plainText">The text to hash.</param>
        /// <param name="salt">The salt used in the hash to mnake it more unique.</param>
        /// <returns>The base64 encoded value of the hash bytes.</returns>
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
        /// Creates a random key.
        /// </summary>
        /// <param name="length">The length of the key.</param>
        /// <returns>The base64 encoded key value.</returns>
        public static string CreateRandomKey(int length)
        {
            var crypto = new RNGCryptoServiceProvider();

            var data = new byte[length];
            crypto.GetNonZeroBytes(data);

            return Convert.ToBase64String(data);
        }
    }
}
