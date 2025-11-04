using Microsoft.AspNetCore.Mvc;
using Smart_trafic_controller_api.DTOs.User;
using Smart_trafic_controller_api.Interfaces;
using Smart_trafic_controller_api.Utilities;

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

        [HttpGet]
        public async Task<IActionResult> GetUserInfo()
        {
            try
            {
                Guid userId;
                
                // Try to get user ID from JWT claims first (if token is in Authorization header)
                var userIdFromClaims = User.FindFirst("sub")?.Value;
                
                if (!string.IsNullOrEmpty(userIdFromClaims))
                {
                    userId = Guid.Parse(userIdFromClaims);
                }
                else
                {
                    // If not found in claims, try to get from cookie
                    string? jwtToken = Request.Cookies["jwtToken"];
                    if (string.IsNullOrEmpty(jwtToken))
                        return Unauthorized("No authentication token found.");
                        
                    userId = JwtHelper.GetUserIdFromToken(jwtToken) ?? 
                             throw new Exception("User ID not found in token.");
                }
                
                var userInfo = await _userService.GetUserByIdAsync(userId);
                if (userInfo == null)
                    return NotFound("User not found.");
        
                return Ok(userInfo);
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