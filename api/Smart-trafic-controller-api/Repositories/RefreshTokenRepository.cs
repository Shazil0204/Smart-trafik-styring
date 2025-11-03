using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Smart_trafic_controller_api.Data;
using Smart_trafic_controller_api.Entities;
using Smart_trafic_controller_api.Interfaces;

namespace Smart_trafic_controller_api.Repositories
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

        public async Task<RefreshToken?> GetByTokenHashAsync(string tokenHash)
        {
            try
            {
                return await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.TokenHash == tokenHash);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the refresh token.", ex);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}