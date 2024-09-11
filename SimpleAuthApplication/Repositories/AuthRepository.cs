using Microsoft.EntityFrameworkCore;
using SimpleAuthApplication.Data;
using SimpleAuthApplication.Models;

namespace SimpleAuthApplication.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserRepository _userRepository;

        public AuthRepository(ApplicationDbContext dbContext, IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
        }

        public async Task<Auth> GetAuthByLoginAsync(string login)
        {
            return await _dbContext.Auths
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Login == login);
        }

        public async Task CreateTokenAsync(Token token)
        {
            await _dbContext.Tokens.AddAsync(token);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateTokenAsync(Token token)
        {
            _dbContext.Tokens.Update(token);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Token> GetTokenAsync(string refreshToken)
        {
            return await _dbContext.Tokens
                               .Include(t => t.Auth)
                               .ThenInclude(a => a.User)
                               .FirstOrDefaultAsync(t => t.RefreshToken == refreshToken);
        }

        public async Task DeactiveTokenAsync(string refreshToken)
        {
            var token = await _dbContext.Tokens.FirstOrDefaultAsync(t => t.RefreshToken == refreshToken);
            var user = await _userRepository.GetUserByIdAsync(token.Auth.UserId);
            if (token != null)
            {
                token.IsActive = false;
                token.UpdatedAt = DateTime.Now;
                token.UpdatedBy = user.Id;

                await _dbContext.SaveChangesAsync();
            }
            
        }
    }
}
