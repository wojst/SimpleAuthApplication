using SimpleAuthApplication.Dtos;

namespace SimpleAuthApplication.Services
{
    public interface IUserService
    {
        Task<UserDto> GetUserAsync(int userId);
        Task<TokenDto> LoginAsync(AuthDto authDto);
        Task<TokenDto> RefreshTokenAsync(string refreshToken);
    }
}