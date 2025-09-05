using Perfumes.DAL.Entities;
using Perfumes.DAL.Repositories;

namespace Perfumes.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        IUserRepository Users { get; }
        ICategoryRepository Categories { get; }
        IBrandRepository Brands { get; }
        ICouponRepository Coupons { get; }
        IOrderRepository Orders { get; }
        IRepository<Cart> Cart { get; }
        IRepository<Wishlist> Wishlist { get; }
        IRepository<OrderDetail> OrderDetails { get; }
        IRepository<ShippingInfo> ShippingInfo { get; }
        IRepository<Payment> Payments { get; }
        IRepository<Review> Reviews { get; }
        IRepository<Inventory> Inventory { get; }
        
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
} 