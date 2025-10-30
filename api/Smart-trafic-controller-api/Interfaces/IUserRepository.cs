using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smart_trafic_controller_api.Entities;

namespace Smart_trafic_controller_api.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(User user);
        Task<User?> LoginUserAsync(string userName, string password);
        Task<bool> SoftDeleteUserAsync(Guid userId);
    }
}