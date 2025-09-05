using Microsoft.EntityFrameworkCore.Storage;
using Perfumes.DAL.Entities;
using Perfumes.DAL.Repositories;

namespace Perfumes.DAL.UnitOfWork
{
    public class UnitOfWork(PerfumesDbContext context) : IUnitOfWork
    {
        private readonly PerfumesDbContext _context = context;
        private IDbContextTransaction? _transaction;

        // Repositories
        private IProductRepository? _products;
        private IUserRepository? _users;
        private ICategoryRepository? _categories;
        private IBrandRepository? _brands;
        private ICouponRepository? _coupons;
        private IOrderRepository? _orders;
        private IRepository<Cart>? _cart;
        private IRepository<Wishlist>? _wishlist;
        private IRepository<OrderDetail>? _orderDetails;
        private IRepository<ShippingInfo>? _shippingInfo;
        private IRepository<Payment>? _payments;
        private IRepository<Review>? _reviews;
        private IRepository<Inventory>? _inventory;

        public IProductRepository Products => _products ??= new ProductRepository(_context);
        public IUserRepository Users => _users ??= new UserRepository(_context);
        public ICategoryRepository Categories => _categories ??= new CategoryRepository(_context);
        public IBrandRepository Brands => _brands ??= new BrandRepository(_context);
        public ICouponRepository Coupons => _coupons ??= new CouponRepository(_context);
        public IOrderRepository Orders => _orders ??= new OrderRepository(_context);
        public IRepository<Cart> Cart => _cart ??= new Repository<Cart>(_context);
        public IRepository<Wishlist> Wishlist => _wishlist ??= new Repository<Wishlist>(_context);
        public IRepository<OrderDetail> OrderDetails => _orderDetails ??= new Repository<OrderDetail>(_context);
        public IRepository<ShippingInfo> ShippingInfo => _shippingInfo ??= new Repository<ShippingInfo>(_context);
        public IRepository<Payment> Payments => _payments ??= new Repository<Payment>(_context);
        public IRepository<Review> Reviews => _reviews ??= new Repository<Review>(_context);
        public IRepository<Inventory> Inventory => _inventory ??= new Repository<Inventory>(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
} 