using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;

namespace PAKNAPI.Common
{
	public class PasswordHasher
	{
		public string GenerateHash(string password, byte[] salt)
		{
			string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
			password: password,
			salt: salt,
			prf: KeyDerivationPrf.HMACSHA1,
			iterationCount: 10000,
			numBytesRequested: 256 / 8));

			return hashed;
		}

		public byte[] GenerateSalt()
		{
			var salt = new byte[32];

			var randomProvider = new RNGCryptoServiceProvider();
			randomProvider.GetBytes(salt);
			return salt;
		}

		public string ConvertByteToString(byte[] salt)
		{
			var stringSalt = Convert.ToBase64String(salt);
			return stringSalt;
		}

		public byte[] SaltToByte(string salt)
		{
			var byteSalt = Convert.FromBase64String(salt);
			return byteSalt;
		}

		public bool AuthenticateUser(string enteredPassword, string storedHash, string storedSalt)
		{
			var saltBytes = Convert.FromBase64String(storedSalt);
			return GenerateHash(enteredPassword, saltBytes) == storedHash;
		}
	}
}