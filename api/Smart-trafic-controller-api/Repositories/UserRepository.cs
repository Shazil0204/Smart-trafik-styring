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
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<User> CreateUserAsync(User user)
        {
            User createdUser = _context.Users.Add(user).Entity;
            await _context.SaveChangesAsync();
            return createdUser;
        }

        public async Task<User?> LoginUserAsync(string userName, string password)
        {
            User? user = await _context.Users
                .Where(u => u.UserName == userName && u.Password == password && !u.IsDeleted)
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<bool> SoftDeleteUserAsync(Guid userId)
        {
            User? user = await _context.Users.FindAsync(userId);
            if (user == null || user.IsDeleted)
            {
                return false;
            }

            user.SoftDelete();
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}