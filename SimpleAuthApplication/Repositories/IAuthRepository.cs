using SimpleAuthApplication.Models;

namespace SimpleAuthApplication.Repositories
{
    public interface IAuthRepository
    {
        Task CreateTokenAsync(Token token);
        Task UpdateTokenAsync(Token token);
        Task<Auth> GetAuthByLoginAsync(string login);
        Task<Token> GetTokenAsync(string refreshToken);
        Task DeactiveTokenAsync(string refreshToken);
    }
}