using Perfumes.DAL.Entities;

namespace Perfumes.DAL.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserWithOrdersAsync(int userId);
        Task<User?> GetUserWithCartAsync(int userId);
        Task<User?> GetUserWithWishlistAsync(int userId);
        Task<IEnumerable<User>> GetActiveUsersAsync();
        Task<bool> IsEmailUniqueAsync(string email);
        Task<User?> GetUserWithReviewsAsync(int userId);
        Task<IEnumerable<User>> GetUsersByRoleAsync(string role);
    }
} 