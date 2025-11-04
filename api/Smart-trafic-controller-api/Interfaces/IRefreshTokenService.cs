using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smart_trafic_controller_api.ValueObjects;

namespace Smart_trafic_controller_api.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<(string Jwt, RefreshTokenValueObject RefreshToken)> LoginAsync(string username, string password);
        Task<(string Jwt, RefreshTokenValueObject RefreshToken)> RefreshTokenAsync(string refreshToken, Guid? userId = null);
        Task<bool> LogoutAsync(string refreshToken, Guid? userId = null);
    }
}