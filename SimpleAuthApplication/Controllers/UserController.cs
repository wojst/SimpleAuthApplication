using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleAuthApplication.Dtos;
using SimpleAuthApplication.Models;
using SimpleAuthApplication.Services;
using System.Security.Claims;

namespace SimpleAuthApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            await _userService.RegisterUserAsync(userRegisterDto);

            return Ok($"User {userRegisterDto.Login} registered.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthDto authDto)
        {
            try
            {
                var tokenDto = await _userService.LoginAsync(authDto);
                return Ok(tokenDto);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            try
            {
                var tokenDto = await _userService.RefreshTokenAsync(refreshToken);
                return Ok(tokenDto);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userDto = await _userService.GetUserAsync(userId);

            return Ok(userDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserUpdateDto userUpdateDto)
        {
            var userIdFromToken = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (userIdFromToken != id)
            {
                return StatusCode(403, "You are not allowed to edit other users' data");
            }

            await _userService.UpdateUserAsync(id, userUpdateDto);
            return Ok(userUpdateDto);
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _userService.DeleteUserAsync(id);
            return Ok($"User deleted");
        }

    }
}
