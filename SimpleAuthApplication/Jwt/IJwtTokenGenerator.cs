using SimpleAuthApplication.Dtos;
using SimpleAuthApplication.Models;

namespace SimpleAuthApplication.Jwt
{
    public interface IJwtTokenGenerator
    {
        TokenDto GenerateToken(User user);
    }
}