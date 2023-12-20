using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;

namespace API.Utils {
    public static class Utilities {
 
        //
        public static string GetHashedValue(string value) {
            // Generate a 128-bit salt using a sequence of cryptographically strong random bytes.
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
            Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: value!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

        //
        public static string GetEncodeValue(string value) {

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
        }

        //
        public static string GetDecodeValue(string value)
        { 
            byte[] bytesDecodificados = Convert.FromBase64String(value);
            string textoDecodificado = Encoding.UTF8.GetString(bytesDecodificados);
            return textoDecodificado;
        }
    }
}