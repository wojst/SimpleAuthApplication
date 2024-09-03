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
            if (auth == null || !BCrypt.Net.BCrypt.Verify(authDto.Password, auth.Password))
            {
                throw new UnauthorizedAccessException("Invalid login or password!");
            }

            var user = await _userRepository.GetUserByLoginAsync(authDto.Login);
            var tokenDto = _jwtTokenGenerator.GenerateToken(user);

            var token = new Token
            {
                RefreshToken = tokenDto.RefreshToken,
                RefreshTokenExpiry = DateTime.UtcNow.AddDays(30),
                AuthId = auth.Id
            };

            await _authRepository.CreateTokenAsync(token);

            return tokenDto;
        }

        public async Task<TokenDto> RefreshTokenAsync(string refreshToken)
        {
            var token = await _authRepository.GetTokenAsync(refreshToken);

            if (token == null || token.RefreshTokenExpiry <= DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token");
            }

            var user = await _userRepository.GetUserByIdAsync(token.Auth.UserId);
            var newTokenDto = _jwtTokenGenerator.GenerateToken(user);

            token.RefreshToken = newTokenDto.RefreshToken;
            token.RefreshTokenExpiry = DateTime.UtcNow.AddDays(30);

            await _authRepository.CreateTokenAsync(token);

            return newTokenDto;

        }

        public async Task RegisterUserAsync(UserRegisterDto userRegisterDto)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRegisterDto.Password);

            var newUser = new User
            {
                FirstName = userRegisterDto.FirstName,
                LastName = userRegisterDto.LastName,
                Age = userRegisterDto.Age,
                JobPosition = userRegisterDto.JobPosition,
                EmploymentType = userRegisterDto.EmploymentType,

                Auth = new Auth
                {
                    Login = userRegisterDto.Login,
                    Password = hashedPassword,
                    Email = userRegisterDto.Email
                }
            };

            await _userRepository.CreateUserAsync(newUser);
        }
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return users.Select(user => new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Age = user.Age,
                JobPosition = user.JobPosition,
                EmploymentType = user.EmploymentType,
                Login = user.Auth.Login,
                Email = user.Auth.Email
            });
        }
    }
}
