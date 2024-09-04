using Microsoft.EntityFrameworkCore;
using SimpleAuthApplication.Data;
using SimpleAuthApplication.Models;

namespace SimpleAuthApplication.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            return await _dbContext.Users
                .Include(u => u.Auth)
                .FirstOrDefaultAsync(u => u.Id == id);

        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _dbContext.Users
                .Include(u => u.Auth)
                .ToListAsync();
        }

        public async Task<User> GetUserByLoginAsync(string login)
        {
            return await _dbContext.Users
                .Include(u => u.Auth)
                .FirstOrDefaultAsync(u => u.Auth.Login == login);
        }

        public async Task CreateUserAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        
        
            
    }
}
