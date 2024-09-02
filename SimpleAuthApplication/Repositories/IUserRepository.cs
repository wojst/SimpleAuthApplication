using SimpleAuthApplication.Models;

namespace SimpleAuthApplication.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByLoginAsync(string login);
        Task CreateUserAsync(User user);
    }
}