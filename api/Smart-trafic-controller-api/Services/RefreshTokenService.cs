using Smart_trafic_controller_api.Entities;
using Smart_trafic_controller_api.Interfaces;
using Smart_trafic_controller_api.Utilities;
using Smart_trafic_controller_api.ValueObjects;

namespace Smart_trafic_controller_api.Services
{
    public class RefreshTokenService(
        IConfiguration config,
        IUserRepository userRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        IRefreshTokenRepository refreshTokenRepository
    ) : IRefreshTokenService
    {
        private readonly Hashing _hashing = new();
        private readonly IConfiguration _config = config;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;
        private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;

        public async Task<(string Jwt, RefreshTokenValueObject RefreshToken)> LoginAsync(
            string username,
            string password
        )
        {
            User? user =
                await _userRepository.GetUserByUserNameAsync(username)
                ?? throw new Exception("Invalid username.");

            if (user.Password == null || !_hashing.VerifyHash(password, user.Password))
            {
                throw new Exception("Invalid password.");
            }

            string jwt = _jwtTokenGenerator.GenerateToken(user.Id);

            RefreshTokenValueObject refreshTokenValue = RefreshTokenValueObject.Create();

            int refreshTokenDays = int.Parse(_config["JwtSettings:RefreshTokenDays"] ?? "1");

            DateTime refreshTokenExpiry = DateTime.UtcNow.AddDays(refreshTokenDays);
            RefreshToken refreshTokenEntity = new RefreshToken(
                user.Id,
                refreshTokenValue,
                refreshTokenExpiry
            );

            await _refreshTokenRepository.AddAsync(refreshTokenEntity);
            await _refreshTokenRepository.SaveChangesAsync();

            return (jwt, refreshTokenValue);
        }

        public async Task<(string Jwt, RefreshTokenValueObject RefreshToken)> RefreshTokenAsync(
            string refreshToken,
            Guid? userId = null
        )
        {
            RefreshToken? existingToken;

            if (userId.HasValue)
            {
                // Much faster - only check tokens for this user
                existingToken = await _refreshTokenRepository.GetByTokenHashAndUserIdAsync(
                    refreshToken,
                    userId.Value
                );
            }
            else
            {
                // Fallback to checking all tokens (slower)
                existingToken = await _refreshTokenRepository.GetByTokenHashAsync(refreshToken);
            }

            if (existingToken == null || !existingToken.IsActive)
            {
                throw new Exception("Invalid refresh token.");
            }

            // Generate new JWT
            string jwt = _jwtTokenGenerator.GenerateToken(existingToken.UserId);

            // Always generate new refresh token for security (token rotation)
            RefreshTokenValueObject newRefreshToken = RefreshTokenValueObject.Create();

            int refreshTokenDays = int.Parse(_config["JwtSettings:RefreshTokenDays"] ?? "1");
            DateTime refreshTokenExpiry = DateTime.UtcNow.AddDays(refreshTokenDays);
            RefreshToken newRefreshTokenEntity = new(
                existingToken.UserId,
                newRefreshToken,
                refreshTokenExpiry
            );

            // Revoke old token and save new one
            existingToken.Revoke();

            await _refreshTokenRepository.UpdateAsync(existingToken);
            await _refreshTokenRepository.AddAsync(newRefreshTokenEntity);
            await _refreshTokenRepository.SaveChangesAsync();

            return (jwt, newRefreshToken);
        }

        public async Task<bool> LogoutAsync(string refreshToken, Guid? userId = null)
        {
            RefreshToken? existingToken;

            if (userId.HasValue)
            {
                // Much faster - only check tokens for this user
                existingToken = await _refreshTokenRepository.GetByTokenHashAndUserIdAsync(
                    refreshToken,
                    userId.Value
                );
            }
            else
            {
                // Fallback to checking all tokens (slower)
                existingToken = await _refreshTokenRepository.GetByTokenHashAsync(refreshToken);
            }

            if (existingToken == null || !existingToken.IsActive)
            {
                throw new Exception("Invalid refresh token.");
            }

            existingToken.Revoke();

            await _refreshTokenRepository.UpdateAsync(existingToken);
            await _refreshTokenRepository.SaveChangesAsync();

            return true;
        }
    }
}
