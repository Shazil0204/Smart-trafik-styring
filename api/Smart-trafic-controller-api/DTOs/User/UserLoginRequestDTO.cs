using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_trafic_controller_api.DTOs.User
{
    public class UserLoginRequestDTO (string userName, string password)
    {
        public string UserName { get; set; } = userName;
        public string Password { get; set; } = password;
    }
}