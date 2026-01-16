using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;
using UrbanNative.Domain.Entities;

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

    public static class PasswordHasher
    {
        private const int SaltSize = 16;   // 128-bit
        private const int HashSize = 32;   // 256-bit
        private const int Iterations = 100000;

        // Generate salt
        public static byte[] GenerateSalt()
        {
            return RandomNumberGenerator.GetBytes(SaltSize);
        }

        // Hash password
        public static byte[] HashPassword(string password, byte[] salt)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            return pbkdf2.GetBytes(HashSize);
        }

        // Verify password
        public static bool VerifyPassword(string password, string storedHashBase64, string storedSaltBase64)
        {
            var storedHash = Convert.FromBase64String(storedHashBase64);
            var storedSalt = Convert.FromBase64String(storedSaltBase64);

            var computed = HashPassword(password, storedSalt);

            return CryptographicOperations.FixedTimeEquals(computed, storedHash);
        }
    }
}
