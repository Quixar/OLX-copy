using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace OLX_copy
{
    public class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 10000;

        public static string GenerateSalt()
        {
            byte[] salt = new byte[SaltSize];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            return Convert.ToBase64String(salt);
        }

        public static string HashPassword(string password, string base64Salt)
        {
            var salt = Convert.FromBase64String(base64Salt);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            var hash = pbkdf2.GetBytes(HashSize);
            return Convert.ToBase64String(hash);
        }

        public static bool VerifyPassword(string password, string base64Salt, string base64Hash)
        {
            string inputHash = HashPassword(password, base64Salt);
            return base64Hash == inputHash;
        }
    }
}
