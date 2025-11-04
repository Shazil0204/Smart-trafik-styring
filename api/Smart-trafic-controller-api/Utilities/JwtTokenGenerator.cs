using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Smart_traffic_controller_api.Interfaces;

namespace Smart_traffic_controller_api.Utilities
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _config;

        public JwtTokenGenerator(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(Guid userId)
        {
            string role = "User";
            IConfigurationSection? jwtSettings = _config.GetSection("JwtSettings");
            Byte[]? key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);
            string? issuer = jwtSettings["Issuer"];
            string? audience = jwtSettings["Audience"];
            int expiresMinutes = int.Parse(jwtSettings["ExpiresMinutes"]!);

            Claim[]? claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(ClaimTypes.Role, role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(
                    JwtRegisteredClaimNames.Iat,
                    DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()
                ),
            };

            JwtSecurityToken? token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256
                )
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
