using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Smart_trafic_controller_api.DTOs.User;
using Smart_trafic_controller_api.Interfaces;

namespace Smart_trafic_controller_api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDTO requestDTO)
        {
            try
            {
                var createdUser = await _userService.CreateUserAsync(requestDTO);
                if (createdUser == null)
                    return BadRequest("User could not be created.");

                return Ok(createdUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginRequestDTO requestDTO)
        {
            try
            {
                UserResponseDTO? user = await _userService.LoginUserAsync(requestDTO);
                if (user == null)
                {
                    return Unauthorized("Invalid username or password.");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("logout/{userId}")]
        public async Task<IActionResult> LogoutUser([FromRoute] Guid userId)
        {
            try
            {
                bool result = await _userService.LogoutUserAsync(userId);
                if (!result)
                {
                    return BadRequest("Logout failed.");
                }
                return Ok("Logout successful.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("soft-delete/{userId}")]
        public async Task<IActionResult> SoftDeleteUser([FromRoute] Guid userId)
        {
            try
            {
                bool result = await _userService.SoftDeleteUserAsync(userId);
                if (!result)
                {
                    return BadRequest("User could not be deleted.");
                }
                return Ok("User deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}