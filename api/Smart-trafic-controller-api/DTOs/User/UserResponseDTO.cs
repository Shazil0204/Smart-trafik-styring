using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_trafic_controller_api.DTOs.User
{
    public class UserResponseDTO(Guid id, string userName, DateTime createdAt)
    {
        public Guid Id { get; set; } = id;
        public string UserName { get; set; } = userName;
        public DateTime CreatedAt { get; set; } = createdAt;
    }
}