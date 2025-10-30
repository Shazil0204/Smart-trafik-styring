using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_trafic_controller_api.Mappers
{
    public static class UserMapper
    {
        public static Entities.User ToEntity(this DTOs.CreateUserRequestDTO dto)
        {
            return new Entities.User(dto.UserName, dto.Password);
        }
    }
}