using System.Text.RegularExpressions;

namespace Smart_trafic_controller_api.Utilities
{
    public partial class Hashing
    {
        [GeneratedRegex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", RegexOptions.Compiled)]
        private static partial Regex PasswordValidationRegex();
        public string IsPasswordValid(string password)
        {
            if (string.IsNullOrEmpty(password) || !PasswordValidationRegex().IsMatch(password))
            {
                return "Password must be at least 8 characters long and include at least one uppercase letter, one lowercase letter, one digit, and one special character.";
            }
            return string.Empty;
        }
        public string HashString(string input)
        {
            try
            {
                return BCrypt.Net.BCrypt.HashPassword(input);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while hashing the string.", ex);
            }
        }

        public bool VerifyHash(string input, string hash)
        {
            try
            {

                return BCrypt.Net.BCrypt.Verify(input, hash);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while verifying the hash.", ex);
            }
        }
    }
}