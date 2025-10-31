using Smart_trafic_controller_api.DTOs.User;
using Smart_trafic_controller_api.Entities;
using Smart_trafic_controller_api.Interfaces;
using Smart_trafic_controller_api.Mappers;
using Smart_trafic_controller_api.Utilities;

namespace Smart_trafic_controller_api.Services
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly Hashing _hashing = new();

        public async Task<UserResponseDTO> CreateUserAsync(CreateUserRequestDTO requestDTO)
        {
            try
            {
                User? userExists = await _userRepository.GetUserByUserNameAsync(requestDTO.UserName);
                if (userExists != null)
                {
                    throw new Exception("User with the same username already exists.");
                }

                requestDTO.Password = _hashing.HashString(requestDTO.Password);

                User user = UserMapper.ToEntity(requestDTO);
                User createdUser = await _userRepository.CreateUserAsync(user) ?? throw new Exception("User could not be created.");
                return UserMapper.ToResponseDTO(createdUser);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the user.", ex);
            }
        }

        public async Task<UserResponseDTO?> LoginUserAsync(UserLoginRequestDTO requestDTO)
        {
            try
            {
                User? user = await _userRepository.GetUserByUserNameAsync(requestDTO.UserName); 
                
                if (user == null || !_hashing.VerifyHash(requestDTO.Password, user.Password))
                {
                    throw new Exception("Invalid username or password.");
                }
                return UserMapper.ToResponseDTO(user);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while logging in the user.", ex);
            }
        }

        public async Task<bool> LogoutUserAsync(Guid userId)
        {
            // TODO: Implement logout logic if needed (e.g., token invalidation)
            return await Task.FromResult(true);
        }

        public async Task<bool> SoftDeleteUserAsync(Guid userId)
        {
            try
            {
                bool result = await _userRepository.SoftDeleteUserAsync(userId);
                if (!result)
                {
                    throw new Exception("User could not be deleted.");
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the user.", ex);
            }
        }
    }
}