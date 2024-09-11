using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using SimpleAuthApplication.Dtos;
using SimpleAuthApplication.Hubs;
using SimpleAuthApplication.Jwt;
using SimpleAuthApplication.Models;
using SimpleAuthApplication.Repositories;
using System.Security.Claims;

namespace SimpleAuthApplication.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthRepository _authRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IHubContext<UserActivityHub> _hubContext;

        public UserService(IUserRepository userRepository, IAuthRepository authRepository, IJwtTokenGenerator jwtTokenGenerator, IHubContext<UserActivityHub> hubContext)
        {
            _userRepository = userRepository;
            _authRepository = authRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _hubContext = hubContext;
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
                CreatedAt = DateTime.Now,
                CreatedBy = Guid.Empty,
                UpdatedAt = DateTime.Now,
                UpdatedBy = Guid.Empty,

                Auth = new Auth
                {
                    Login = userRegisterDto.Login,
                    Password = hashedPassword,
                    Email = userRegisterDto.Email,
                    CreatedAt = DateTime.Now,
                    CreatedBy = Guid.Empty,
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = Guid.Empty
                }
            };

            await _userRepository.CreateUserAsync(newUser);       

            await _hubContext.Clients.All.SendAsync("ReceiveUserActivity", $"{newUser.FirstName} {newUser.LastName} has registered in.");
        }

        public async Task<TokenDto> LoginAsync(AuthDto authDto)
        {
            var user = await _userRepository.GetUserByLoginAsync(authDto.Login);
            var auth = await _authRepository.GetAuthByLoginAsync(authDto.Login);
            if (auth == null || !BCrypt.Net.BCrypt.Verify(authDto.Password, auth.Password))
            {
                await _hubContext.Clients.All.SendAsync("ReceiveUserActivity", $"Login attempt failed.");
                throw new UnauthorizedAccessException("Invalid login or password!");
            }

            await _hubContext.Clients.All.SendAsync("ReceiveUserActivity", $"{user.FirstName} {user.LastName} has logged in.");

            var tokenDto = _jwtTokenGenerator.GenerateToken(user);

            var token = new Token
            {
                RefreshToken = tokenDto.RefreshToken,
                RefreshTokenExpiry = DateTime.UtcNow.AddDays(30),
                AuthId = auth.Id,
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedBy = user.Id,
                UpdatedAt = DateTime.Now,
                UpdatedBy = user.Id

            };

            await _authRepository.CreateTokenAsync(token);

            return tokenDto;
        }

        public async Task<TokenDto> RefreshTokenAsync(string refreshToken)
        {
            // Istniejący token
            var token = await _authRepository.GetTokenAsync(refreshToken);

            // Sprawdzanie czy token istnieje lub nie wygasł
            if (token == null || token.RefreshTokenExpiry <= DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Refresh token has expired");
            }

            // Dezaktywacja starego tokenu
            await _authRepository.DeactiveTokenAsync(refreshToken);

            // Znajdź użytkownika dla istniejącego tokenu i wygeneruj nowy
            var user = await _userRepository.GetUserByIdAsync(token.Auth.UserId);
            var newTokenDto = _jwtTokenGenerator.GenerateToken(user);

            var newToken = new Token
            {
                RefreshToken = newTokenDto.RefreshToken,
                RefreshTokenExpiry = DateTime.UtcNow.AddDays(30),
                AuthId = token.AuthId,
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedBy = user.Id,
                UpdatedAt = DateTime.Now,
                UpdatedBy = user.Id
            };

            await _authRepository.CreateTokenAsync(newToken);

            await _hubContext.Clients.All.SendAsync("ReceiveUserActivity", $"{user.FirstName} {user.LastName} has refreshed token.");

            return newTokenDto;
        }

        public async Task<UserDto> GetUserAsync(Guid userId)
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

        public async Task UpdateUserAsync(Guid userId, UserUpdateDto userUpdateDto)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found!");
            }

            user.FirstName = userUpdateDto.FirstName ?? user.FirstName;
            user.LastName = userUpdateDto.LastName ?? user.LastName;
            user.Age = userUpdateDto.Age != 0 ? userUpdateDto.Age : user.Age;
            user.JobPosition = userUpdateDto.JobPosition ?? user.JobPosition;
            user.EmploymentType = userUpdateDto.EmploymentType ?? user.EmploymentType;

            await _userRepository.UpdateUserAsync(user);
            await _hubContext.Clients.All.SendAsync("ReceiveUserActivity", $"{user.FirstName} {user.LastName} has updated own data.");
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            await _userRepository.DeleteUserAsync(id);
            await _hubContext.Clients.All.SendAsync("ReceiveUserActivity", $"{user.FirstName} {user.LastName} deleted.");
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
