using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PracticalTest.Utils
{
    public class PasswordSecurity
    {
        const int iterations = 1000;
        const int saltSize = 24;
        public static bool IsPasswordMatch(string passwordHash, string salt, string password2)
        {
           
            byte[] passwordHashBytes = Encoding.Default.GetBytes(passwordHash);
            byte[] passwordBytes2 = Encoding.Default.GetBytes(password2);

            byte[] saltBytes = Encoding.Default.GetBytes(salt); //GenerateSalt(saltSize);

            byte[] password2HashBytes = GenerateHash(passwordBytes2, saltBytes, iterations, saltSize);

            return (ByteArrayCompare(passwordHashBytes, password2HashBytes));
        }

        public static byte[] GenerateSalt(int length)
        {
            var bytes = new byte[length];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
            }
            return bytes;
        }

        public static byte[] GenerateHash(byte[] password, byte[] salt, int iterations, int length)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                return deriveBytes.GetBytes(length);
            }
        }

        public static bool ByteArrayCompare(byte[] a1, byte[] a2)
        {
            return StructuralComparisons.StructuralEqualityComparer.Equals(a1, a2);
        }

    }
}
