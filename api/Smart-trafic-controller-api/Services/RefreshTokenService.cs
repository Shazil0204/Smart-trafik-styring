using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smart_trafic_controller_api.Entities;
using Smart_trafic_controller_api.Interfaces;
using Smart_trafic_controller_api.Utilities;
using Smart_trafic_controller_api.ValueObjects;

namespace Smart_trafic_controller_api.Services
{
    public class RefreshTokenService(IConfiguration config, IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator, IRefreshTokenRepository refreshTokenRepository) : IRefreshTokenService
    {
        private readonly Hashing _hashing = new();
        private readonly IConfiguration _config = config;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;
        private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
        public async Task<(string Jwt, RefreshTokenValueObject RefreshToken)> LoginAsync(string username, string password)
        {
            User? user = await _userRepository.GetUserByUserNameAsync(username) ?? throw new Exception("Invalid username.");

            if (user.Password == null || !_hashing.VerifyHash(password, user.Password))
            {
                throw new Exception("Invalid password.");
            }

            string jwt = _jwtTokenGenerator.GenerateToken(user.Id);

            RefreshTokenValueObject refreshTokenValue = RefreshTokenValueObject.Create();

            int refreshTokenDays = int.Parse(_config["JwtSettings:RefreshTokenExpirationDays"] ?? "1");

            DateTime refreshTokenExpiry = DateTime.UtcNow.AddDays(refreshTokenDays);
            RefreshToken refreshTokenEntity = new RefreshToken(user.Id, refreshTokenValue, refreshTokenExpiry);

            await _refreshTokenRepository.AddAsync(refreshTokenEntity);
            await _refreshTokenRepository.SaveChangesAsync();

            return (jwt, refreshTokenValue);
        }
    }
}