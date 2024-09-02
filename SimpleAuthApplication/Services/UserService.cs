using SimpleAuthApplication.Dtos;
using SimpleAuthApplication.Jwt;
using SimpleAuthApplication.Models;
using SimpleAuthApplication.Repositories;

namespace SimpleAuthApplication.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthRepository _authRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public UserService(IUserRepository userRepository, IAuthRepository authRepository, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _authRepository = authRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<UserDto> GetUserAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found!");
            }

            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Age = user.Age,
                JobPosition = user.JobPosition,
                EmploymentType = user.EmploymentType,
                Login = user.Auth.Login,
                Email = user.Auth.Email
            };
        }

        public async Task<TokenDto> LoginAsync(AuthDto authDto)
        {
            var auth = await _authRepository.GetAuthByLoginAsync(authDto.Login);
            if (auth == null || BCrypt.Net.BCrypt.Verify(authDto.Password, auth.Password))
            {
                throw new UnauthorizedAccessException("Invalid login or password!");
            }

            var user = await _userRepository.GetUserByLoginAsync(authDto.Login);
            var tokenDto = _jwtTokenGenerator.GenerateToken(user);

            var token = new Token
            {
                AccessToken = tokenDto.AccessToken,
                RefreshToken = tokenDto.RefreshToken,
                RefreshTokenExpiry = DateTime.UtcNow.AddDays(30),
                AuthId = auth.Id
            };

            await _authRepository.CreateTokenAsync(token);

            return tokenDto;
        }

        public async Task<TokenDto> RefreshTokenAsync(string refreshToken)
        {
            
        }
    }
}
