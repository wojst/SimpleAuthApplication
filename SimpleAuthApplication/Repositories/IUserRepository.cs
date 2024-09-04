using SimpleAuthApplication.Models;

namespace SimpleAuthApplication.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(Guid id);
        Task<User> GetUserByLoginAsync(string login);
        Task CreateUserAsync(User user);
    }
}