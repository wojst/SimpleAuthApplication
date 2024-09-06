using Microsoft.EntityFrameworkCore;
using SimpleAuthApplication.Data;
using SimpleAuthApplication.Models;

namespace SimpleAuthApplication.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AuthRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
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
            if (token != null)
            {
                token.IsActive = false;
                await _dbContext.SaveChangesAsync();
            }
            
        }
    }
}
