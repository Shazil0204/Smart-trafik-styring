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

                string validationMessage = _hashing.IsPasswordValid(requestDTO.Password);
                if (!string.IsNullOrEmpty(validationMessage))
                {
                    throw new Exception(validationMessage);
                }

                requestDTO.Password = _hashing.HashString(requestDTO.Password);

                User user = UserMapper.ToEntity(requestDTO);
                User createdUser = await _userRepository.CreateUserAsync(user);
                return UserMapper.ToResponseDTO(createdUser);
            }

            catch (Exception)
            {
                throw;
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
                
                // Additional login logic can be added here (e.g., generating tokens)
                return UserMapper.ToResponseDTO(user);
            }
            catch (Exception)
            {
                throw;
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
            catch (Exception)
            {
                throw;
            }
        }
    }
}