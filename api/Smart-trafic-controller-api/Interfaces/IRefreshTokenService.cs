using Smart_traffic_controller_api.ValueObjects;

namespace Smart_traffic_controller_api.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<(string Jwt, RefreshTokenValueObject RefreshToken)> LoginAsync(
            string username,
            string password
        );
        Task<(string Jwt, RefreshTokenValueObject RefreshToken)> RefreshTokenAsync(
            string refreshToken,
            Guid? userId = null
        );
        Task<bool> LogoutAsync(string refreshToken, Guid? userId = null);
    }
}
