using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smart_trafic_controller_api.Interfaces;

namespace Smart_trafic_controller_api.Services
{
    public class RefreshTokenService(IConfiguration config, IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator, IRefreshTokenRepository refreshTokenRepository) : IRefreshTokenService
    {
        private readonly IConfiguration _config = config;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;
        private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
    }
}