using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Smart_trafic_controller_api.DTOs.User;
using Smart_trafic_controller_api.Interfaces;

namespace Smart_trafic_controller_api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IConfiguration configuration, IRefreshTokenService refreshTokenService) : ControllerBase
    {
        private readonly IConfiguration _config = configuration;
        private readonly IRefreshTokenService _refreshTokenService = refreshTokenService;

        public async Task<IActionResult> Login([FromBody] LoginRequestDto requestDto)
        {
            try
            {
                var result = await _refreshTokenService.LoginAsync(requestDto.Email, requestDto.Password);
                SetRefreshTokenCookie(result.RefreshToken.Raw);
                return Ok(new { Token = result.Jwt });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private void SetRefreshTokenCookie(string refreshToken)
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
                    Expires = DateTimeOffset.UtcNow.AddDays(refreshTokenDays)
                }
            );
        }
    }
}