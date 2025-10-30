using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_trafic_controller_api.Utilities
{
    public class Hashing
    {
        public string HashString(string input)
        {
            return BCrypt.Net.BCrypt.HashPassword(input);
        }

        public bool VerifyHash(string input, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(input, hash);
        }
    }
}