using System.Text;

namespace Practica_API_JWT.Services
{
    public class EmailNormalized
    {
        public static string NormalizarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El email no puede estar vacío.");

            string normalizado = email.Trim().ToLowerInvariant();
            normalizado = normalizado.Normalize(NormalizationForm.FormC);

            return normalizado;
        }
    }
}
