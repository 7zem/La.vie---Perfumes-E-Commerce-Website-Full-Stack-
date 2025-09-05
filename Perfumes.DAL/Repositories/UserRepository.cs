using Microsoft.EntityFrameworkCore;
using Perfumes.DAL.Entities;

namespace Perfumes.DAL.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(PerfumesDbContext context) : base(context)
        {
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
        }

        public async Task<User?> GetUserWithOrdersAsync(int userId)
        {
            return await _dbSet
                .Include(u => u.Orders)
                .ThenInclude(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(u => u.UserId == userId && u.IsActive);
        }

        public async Task<User?> GetUserWithCartAsync(int userId)
        {
            return await _dbSet
                .Include(u => u.CartItems)
                .ThenInclude(c => c.Product)
                .ThenInclude(p => p.Brand)
                .FirstOrDefaultAsync(u => u.UserId == userId && u.IsActive);
        }

        public async Task<User?> GetUserWithWishlistAsync(int userId)
        {
            return await _dbSet
                .Include(u => u.WishlistItems)
                .ThenInclude(w => w.Product)
                .ThenInclude(p => p.Brand)
                .FirstOrDefaultAsync(u => u.UserId == userId && u.IsActive);
        }

        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            return await _dbSet
                .Where(u => u.IsActive)
                .ToListAsync();
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return !await _dbSet.AnyAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserWithReviewsAsync(int userId)
        {
            return await _dbSet
                .Include(u => u.Reviews)
                .ThenInclude(r => r.Product)
                .FirstOrDefaultAsync(u => u.UserId == userId && u.IsActive);
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(string role)
        {
            return await _dbSet
                .Where(u => u.Role == role && u.IsActive)
                .ToListAsync();
        }
    }
} 