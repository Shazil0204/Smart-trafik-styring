using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smart_trafic_controller_api.DTOs;
using Smart_trafic_controller_api.Entities;

namespace Smart_trafic_controller_api.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDTO> CreateUserAsync(CreateUserRequestDTO dTO);
        Task<UserResponseDTO?> LoginUserAsync(UserLoginRequestDTO dTO);
        Task<bool> LogoutUserAsync(Guid userId);
        Task<bool> SoftDeleteUserAsync(Guid userId);
    }
}