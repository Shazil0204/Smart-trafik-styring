using System.Security.Cryptography;
using System.Text;

namespace Smart_traffic_controller_api.ValueObjects
{
    public class RefreshTokenValueObject
    {
        public string Hashed { get; private set; }
        public string Raw { get; private set; }

        private RefreshTokenValueObject(string hashed, string raw)
        {
            Hashed = hashed;
            Raw = raw;
        }

        // Generate new refresh token
        public static RefreshTokenValueObject Create(int size = 64)
        {
            byte[] randomBytes = new byte[size];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            string rawToken = Convert.ToBase64String(randomBytes);

            // Use SHA256 for fast database lookups (deterministic hash)
            string hashedToken = ComputeSha256Hash(rawToken);

            return new RefreshTokenValueObject(hashedToken, rawToken);
        }

        // Fast lookup hash for database queries
        public static string ComputeSha256Hash(string rawToken)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawToken));
                return Convert.ToBase64String(bytes);
            }
        }

        // Verify raw token against hashed value
        public bool Verify(string token)
        {
            return Hashed == ComputeSha256Hash(token);
        }
    }
}
