using Account.Core.Abstractions;
using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

using static Account.Core.Security.Consts;

namespace Account.Core.Security
{
    internal sealed class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            var salt = new byte[SaltSize];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var hash = HashPassword(password, salt);
            var combinedBytes = new byte[salt.Length + hash.Length];
            Array.Copy(salt, 0, combinedBytes, 0, salt.Length);
            Array.Copy(hash, 0, combinedBytes, salt.Length, hash.Length);

            return Convert.ToBase64String(combinedBytes);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            byte[] combinedBytes = Convert.FromBase64String(hashedPassword);
            byte[] salt = new byte[SaltSize];
            byte[] hash = new byte[HashSize];
            Array.Copy(combinedBytes, 0, salt, 0, SaltSize);
            Array.Copy(combinedBytes, SaltSize, hash, 0, HashSize);
            byte[] newHash = HashPassword(password, salt);

            return CryptographicOperations.FixedTimeEquals(hash, newHash);
        }

        private static byte[] HashPassword(string password, byte[] salt)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = DegreeOfParallelism,
                Iterations = Iterations,
                MemorySize = MemorySize
            };

            return argon2.GetBytes(HashSize);
        }
    }
}