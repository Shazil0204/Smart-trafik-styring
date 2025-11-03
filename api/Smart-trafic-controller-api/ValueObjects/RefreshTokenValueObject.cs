using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Smart_trafic_controller_api.ValueObjects
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
            string hashedToken = BCrypt.Net.BCrypt.HashPassword(rawToken);

            return new RefreshTokenValueObject(rawToken, hashedToken);
        }

        // Verify raw token against hashed value
        public bool Verify(string token)
        {
            return BCrypt.Net.BCrypt.Verify(token, Hashed);
        }
    }
}