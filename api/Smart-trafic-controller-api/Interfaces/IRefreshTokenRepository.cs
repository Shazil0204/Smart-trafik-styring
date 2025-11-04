using Smart_traffic_controller_api.Entities;

namespace Smart_traffic_controller_api.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken refreshToken);
        Task<RefreshToken?> GetByTokenHashAsync(string tokenHash);
        Task<RefreshToken?> GetByTokenHashAndUserIdAsync(string rawToken, Guid userId);
        Task UpdateAsync(RefreshToken refreshToken);
        Task SaveChangesAsync();
    }
}
