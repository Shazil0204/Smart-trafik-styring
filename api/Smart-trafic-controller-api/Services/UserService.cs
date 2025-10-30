using Smart_trafic_controller_api.DTOs;
using Smart_trafic_controller_api.Entities;
using Smart_trafic_controller_api.Interfaces;
using Smart_trafic_controller_api.Mappers;

namespace Smart_trafic_controller_api.Services
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<User> CreateUserAsync(CreateUserRequestDTO requestDTO)
        {
            bool userExists = await _userRepository.GetUserByUserNameAsync(requestDTO.UserName);
            if (userExists)
            {
                throw new Exception("User with the same username already exists.");
            }
            User user = UserMapper.ToEntity(requestDTO);
            User createdUser = await _userRepository.CreateUserAsync(user) ?? throw new Exception("User could not be created.");
            return createdUser;
        }

        public async Task<User?> LoginUserAsync(UserLoginRequestDTO requestDTO)
        {
            User? user = await _userRepository.LoginUserAsync(requestDTO.UserName, requestDTO.Password);
            if (user == null)
            {
                throw new Exception("Invalid username or password.");
            }
            return user;
        }

        public async Task<bool> LogoutUserAsync(Guid userId)
        {
            // Implement logout logic if needed (e.g., token invalidation)
            return await Task.FromResult(true);
        }

        public async Task<bool> SoftDeleteUserAsync(Guid userId)
        {
            bool result = await _userRepository.SoftDeleteUserAsync(userId);
            if (!result)
            {
                throw new Exception("User could not be deleted.");
            }
            return result;
        }
    }
}