using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_trafic_controller_api.Mappers
{
    public static class UserMapper
    {
        public static Entities.User ToEntity(this DTOs.User.CreateUserRequestDTO dto)
        {
            return new Entities.User(dto.UserName, dto.Password);
        }

        public static DTOs.User.UserResponseDTO ToResponseDTO(this Entities.User user)
        {
            return new DTOs.User.UserResponseDTO(user.Id, user.UserName, user.CreatedAt);
        }
    }
}