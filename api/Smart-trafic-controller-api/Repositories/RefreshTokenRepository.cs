using Microsoft.EntityFrameworkCore;
using Smart_traffic_controller_api.Data;
using Smart_traffic_controller_api.Entities;
using Smart_traffic_controller_api.Interfaces;

namespace Smart_traffic_controller_api.Repositories
{
    public class RefreshTokenRepository(AppDbContext context) : IRefreshTokenRepository
    {
        private readonly AppDbContext _context = context;

        public async Task AddAsync(RefreshToken refreshToken)
        {
            try
            {
                await _context.RefreshTokens.AddAsync(refreshToken);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the refresh token.", ex);
            }
        }

        public async Task<RefreshToken?> GetByTokenHashAsync(string rawToken)
        {
            try
            {
                // Now we can do fast database lookup with SHA256 hash
                string hashedToken =
                    Smart_traffic_controller_api.ValueObjects.RefreshTokenValueObject.ComputeSha256Hash(
                        rawToken
                    );

                return await _context.RefreshTokens.FirstOrDefaultAsync(rt =>
                    rt.TokenHash == hashedToken && !rt.IsRevoked && rt.ExpiresAt > DateTime.UtcNow
                );
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the refresh token.", ex);
            }
        }

        public async Task<RefreshToken?> GetByTokenHashAndUserIdAsync(string rawToken, Guid userId)
        {
            try
            {
                // Fast database lookup with SHA256 hash + UserId filter
                string hashedToken =
                    Smart_traffic_controller_api.ValueObjects.RefreshTokenValueObject.ComputeSha256Hash(
                        rawToken
                    );

                return await _context.RefreshTokens.FirstOrDefaultAsync(rt =>
                    rt.UserId == userId
                    && rt.TokenHash == hashedToken
                    && !rt.IsRevoked
                    && rt.ExpiresAt > DateTime.UtcNow
                );
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the refresh token.", ex);
            }
        }

        public async Task UpdateAsync(RefreshToken refreshToken)
        {
            try
            {
                _context.RefreshTokens.Update(refreshToken);
                await Task.CompletedTask; // Fix the async warning
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the refresh token.", ex);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
