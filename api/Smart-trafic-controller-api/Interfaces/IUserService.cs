using Smart_traffic_controller_api.DTOs.User;
using Smart_traffic_controller_api.Entities;

namespace Smart_traffic_controller_api.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDTO> CreateUserAsync(CreateUserRequestDTO dTO);
        Task<User?> GetUserByIdAsync(Guid userId);
        Task<bool> SoftDeleteUserAsync(Guid userId);
    }
}
