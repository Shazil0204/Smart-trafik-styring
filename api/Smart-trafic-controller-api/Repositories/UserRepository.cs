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
            try
            {
                User createdUser = _context.Users.Add(user).Entity;
                await _context.SaveChangesAsync();
                return createdUser;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the user.", ex);
            }
        }

        public Task<User?> GetUserByIdAsync(Guid userId)
        {
            try
            {
                return _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the user.", ex);
            }
        }

        public async Task<User?> GetUserByUserNameAsync(string userName)
        {
            try
            {
                User? user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the user.", ex);
            }
        }

        public async Task<bool> SoftDeleteUserAsync(Guid userId)
        {
            try
            {
                User? user = await _context.Users.FindAsync(userId);
                if (user == null || user.IsDeleted)
                {
                    throw new Exception("User does not exist or has already been deleted.");
                }

                user.SoftDelete();
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the user.", ex);
            }
        }
    }
}
