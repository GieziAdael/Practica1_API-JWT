using System.Security.Cryptography;

namespace Practica_API_JWT.Services
{
    public class PasswordHash
    {
        // Tamaños y parámetros de seguridad
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100_000;

        private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA256;

        //Metodo 1: Hashear una contraseña nueva (para registro)
        public static string Hash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] key = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                Iterations,
                HashAlgorithm,
                KeySize
            );

            return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(key)}";
        }

        //Metodo 2: Verificar contraseña (para login)
        public static bool Verify(string password, string hashedPassword)
        {
            var parts = hashedPassword.Split('.', 3);
            if (parts.Length != 3)
                return false;

            var iterations = Convert.ToInt32(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var hash = Convert.FromBase64String(parts[2]);

            byte[] keyToCheck = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                iterations,
                HashAlgorithm,
                hash.Length
            );

            return CryptographicOperations.FixedTimeEquals(hash, keyToCheck);
        }
    }
}
