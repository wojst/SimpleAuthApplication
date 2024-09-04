using SimpleAuthApplication.Dtos;
using SimpleAuthApplication.Models;

namespace SimpleAuthApplication.Services
{
    public interface IUserService
    {
        Task<UserDto> GetUserAsync(Guid userId);
        Task<TokenDto> LoginAsync(AuthDto authDto);
        Task RegisterUserAsync(UserRegisterDto userRegisterDto);
        Task<TokenDto> RefreshTokenAsync(string refreshToken);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
    }
}