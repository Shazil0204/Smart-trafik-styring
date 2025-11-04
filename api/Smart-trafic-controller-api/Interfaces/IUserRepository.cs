using Smart_traffic_controller_api.Entities;

namespace Smart_traffic_controller_api.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUserNameAsync(string userName);
        Task<User> CreateUserAsync(User user);
        Task<User?> GetUserByIdAsync(Guid userId);
        Task<bool> SoftDeleteUserAsync(Guid userId);
    }
}
