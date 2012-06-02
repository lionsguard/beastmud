
using System;

namespace Beast.Security
{
	public struct AuthToken
	{
		public const char TokenSeparator = '|';

		public static readonly AuthToken Invalid = new AuthToken(string.Empty, string.Empty);

		public string ConnectionId;
		public string IpAddress;
		public DateTime DateCreated;

		public AuthToken(string connectionId, string ipAddress)
		{
			ConnectionId = connectionId;
			IpAddress = ipAddress;
			DateCreated = DateTime.UtcNow;
		}

		public string Encrypt()
		{
			return Cryptography.Encrypt(string.Concat(ConnectionId, TokenSeparator, IpAddress, TokenSeparator, DateCreated.Ticks));
		}

		public static bool TryParse(string encryptedToken, out AuthToken authToken)
		{
			authToken = Invalid;
			if (string.IsNullOrEmpty(encryptedToken))
				return false;

			string decryptedToken;
			try
			{
				decryptedToken = Cryptography.Decrypt(encryptedToken);
			}
			catch (Exception)
			{
				return false;
			}

			var parts = decryptedToken.Split(TokenSeparator);
			if (parts.Length < 3)
				return false;

			long ticks;
			if (!long.TryParse(parts[2], out ticks))
				return false;

			authToken = new AuthToken(parts[0], parts[1]);
			authToken.DateCreated = new DateTime(ticks);
			return true;
		}
	}
}
