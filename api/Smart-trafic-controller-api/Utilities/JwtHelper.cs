using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Smart_trafic_controller_api.Utilities
{
    public static class JwtHelper
    {
        public static Guid? GetUserIdFromToken(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadJwtToken(token);

                var userIdClaim = jsonToken.Claims.FirstOrDefault(x =>
                    x.Type == JwtRegisteredClaimNames.Sub
                );

                if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    return userId;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
