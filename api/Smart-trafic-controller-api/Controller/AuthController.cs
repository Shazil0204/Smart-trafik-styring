using Microsoft.AspNetCore.Mvc;
using Smart_trafic_controller_api.DTOs.User;
using Smart_trafic_controller_api.Interfaces;
using Smart_trafic_controller_api.Utilities;

namespace Smart_trafic_controller_api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(
        IConfiguration configuration,
        IRefreshTokenService refreshTokenService
    ) : ControllerBase
    {
        private readonly IConfiguration _config = configuration;
        private readonly IRefreshTokenService _refreshTokenService = refreshTokenService;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto requestDto)
        {
            try
            {
                var result = await _refreshTokenService.LoginAsync(
                    requestDto.Username,
                    requestDto.Password
                );

                // Set both tokens as cookies
                SetJwtCookie(result.Jwt);
                SetRefreshTokenCookie(result.RefreshToken.Raw);

                return Ok(new { Message = "Login successful" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                string? refreshToken = Request.Cookies["refreshToken"];
                if (string.IsNullOrEmpty(refreshToken))
                {
                    return Unauthorized("Refresh token is missing.");
                }

                // Try to get userId from current JWT token in Authorization header
                Guid? userId = null;
                if (Request.Headers.ContainsKey("Authorization"))
                {
                    var authHeader = Request.Headers["Authorization"].ToString();
                    if (authHeader.StartsWith("Bearer "))
                    {
                        var jwt = authHeader.Substring(7);
                        userId = JwtHelper.GetUserIdFromToken(jwt);
                    }
                }

                var result = await _refreshTokenService.RefreshTokenAsync(refreshToken, userId);

                if (result.Jwt == null || result.RefreshToken == null)
                {
                    return Unauthorized("Invalid refresh token.");
                }

                // Set both new tokens as cookies
                SetJwtCookie(result.Jwt);
                SetRefreshTokenCookie(result.RefreshToken.Raw);

                return Ok(new { Message = "Token refreshed successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                string? refreshToken = Request.Cookies["refreshToken"];
                if (string.IsNullOrEmpty(refreshToken))
                {
                    return Unauthorized("Refresh token is missing.");
                }

                // Try to get userId from current JWT token in Authorization header
                Guid? userId = null;
                if (Request.Headers.ContainsKey("Authorization"))
                {
                    var authHeader = Request.Headers["Authorization"].ToString();
                    if (authHeader.StartsWith("Bearer "))
                    {
                        var jwt = authHeader.Substring(7);
                        userId = JwtHelper.GetUserIdFromToken(jwt);
                    }
                }

                await _refreshTokenService.LogoutAsync(refreshToken, userId);

                // Clear both cookies
                Response.Cookies.Delete("jwtToken");
                Response.Cookies.Delete("refreshToken");

                return Ok(new { Message = "Logged out successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("status")]
        public IActionResult GetAuthStatus()
        {
            try
            {
                string? jwtToken = Request.Cookies["jwtToken"];
                string? refreshToken = Request.Cookies["refreshToken"];

                if (string.IsNullOrEmpty(jwtToken) && string.IsNullOrEmpty(refreshToken))
                {
                    return Ok(new { IsAuthenticated = false, Message = "No tokens found" });
                }

                // Check if JWT is still valid
                bool jwtValid = false;
                Guid? userId = null;

                if (!string.IsNullOrEmpty(jwtToken))
                {
                    userId = JwtHelper.GetUserIdFromToken(jwtToken);
                    jwtValid = userId.HasValue;
                }

                // Check refresh token status if JWT is invalid or we want to check refresh token health
                bool refreshTokenValid = false;
                bool refreshTokenNearExpiry = false;

                if (!string.IsNullOrEmpty(refreshToken))
                {
                    // For now, just check if we can extract user from JWT or assume it's valid
                    // In a real implementation, you'd validate the refresh token against DB
                    refreshTokenValid = true; // Simplified for now
                }

                return Ok(
                    new
                    {
                        IsAuthenticated = jwtValid || refreshTokenValid,
                        JwtValid = jwtValid,
                        RefreshTokenValid = refreshTokenValid,
                        RefreshTokenNearExpiry = refreshTokenNearExpiry,
                        UserId = userId,
                    }
                );
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private void SetRefreshTokenCookie(string refreshToken)
        {
            try
            {
                int refreshTokenDays = int.Parse(_config["JwtSettings:RefreshTokenDays"]!);

                Response.Cookies.Append(
                    "refreshToken",
                    refreshToken,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddDays(refreshTokenDays),
                    }
                );
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "An error occurred while setting the refresh token cookie.",
                    ex
                );
            }
        }

        private void SetJwtCookie(string jwtToken)
        {
            try
            {
                int jwtExpiresMinutes = int.Parse(_config["JwtSettings:ExpiresMinutes"]!);

                Response.Cookies.Append(
                    "jwtToken",
                    jwtToken,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddMinutes(jwtExpiresMinutes),
                    }
                );
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while setting the JWT token cookie.", ex);
            }
        }
    }
}
