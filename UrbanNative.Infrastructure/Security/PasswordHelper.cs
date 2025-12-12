using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace UrbanNative.Infrastructure.Security
{
    public static class PasswordHelper
    {
        // Generate PBKDF2 hash + random salt
        public static (byte[] hash, byte[] salt) CreateHash(
            string password,
            int saltSize = 16,
            int hashSize = 32,
            int iterations = 100_000)
        {
            var salt = RandomNumberGenerator.GetBytes(saltSize);

            var hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: iterations,
                numBytesRequested: hashSize);

            return (hash, salt);
        }

        // Verify password
        public static bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (storedHash == null || storedHash.Length == 0) return false;
            if (storedSalt == null || storedSalt.Length == 0) return false;

            var hashToCheck = KeyDerivation.Pbkdf2(
                password: password,
                salt: storedSalt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100_000,
                numBytesRequested: storedHash.Length);

            return CryptographicOperations.FixedTimeEquals(hashToCheck, storedHash);
        }
    }
}
