namespace Beast.Security
{
	/// <summary>
	/// Defines the types of encryption algorithms that can be used with the Registry class and Security framework.
	/// </summary>
	public enum EncryptionAlgorithm
	{
		/// <summary>
		/// Specifies that the cryptography service should not use an encryption or decryption algorithm to store the information.
		/// </summary>
		None,
		/// <summary>
		/// Specifies that the cryptography service should use the DES encryption algorithm to encrypt and decrypt information.
		/// </summary>
		Des,
		/// <summary>
		/// Specifies that the cryptography service should use the RC2 encryption algorithm to encrypt and decrypt information.
		/// </summary>
		Rc2,
		/// <summary>
		/// Specifies that the cryptography service should use the Rijndael encryption algorithm to encrypt and decrypt information.
		/// </summary>
		Rijndael,
		/// <summary>
		/// Specifies that the cryptography service should use the Triple DES encryption algorithm to encrypt and decrypt information.
		/// </summary>
		TripleDes
	}
}
