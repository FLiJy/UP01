using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UP01.Core
{
    public static class PasswordService
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100000;

        public static string HashPassword(string password)
        {
            byte[] salt = new byte[SaltSize];

            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(salt);
            }

            using (var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                Iterations))
            {
                byte[] hash = pbkdf2.GetBytes(HashSize);

                return Convert.ToBase64String(salt)
                       + ":"
                       + Convert.ToBase64String(hash);
            }
        }

        public static bool VerifyPassword(
            string password,
            string storedHash)
        {
            try
            {
                string[] parts = storedHash.Split(':');

                if (parts.Length != 2)
                    return false;

                byte[] salt =
                    Convert.FromBase64String(parts[0]);

                byte[] savedHash =
                    Convert.FromBase64String(parts[1]);

                using (var pbkdf2 = new Rfc2898DeriveBytes(
                    password,
                    salt,
                    Iterations))
                {
                    byte[] hash = pbkdf2.GetBytes(HashSize);

                    return CompareBytes(hash, savedHash);
                }
            }
            catch
            {
                return false;
            }
        }

        private static bool CompareBytes(
            byte[] first,
            byte[] second)
        {
            if (first.Length != second.Length)
                return false;

            bool result = true;

            for (int i = 0; i < first.Length; i++)
            {
                result &= first[i] == second[i];
            }

            return result;
        }
    }
}