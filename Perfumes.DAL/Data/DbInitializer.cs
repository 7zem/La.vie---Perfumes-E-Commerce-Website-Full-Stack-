using Microsoft.EntityFrameworkCore;
using Perfumes.DAL.Entities;
using BCrypt.Net;

namespace Perfumes.DAL.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(PerfumesDbContext context)
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Seed Admin User
            if (!await context.Users.AnyAsync(u => u.Email == "admin@perfumes.com"))
            {
                var adminUser = new User
                {
                    Name = "Admin",
                    Email = "admin@perfumes.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    Role = "Admin",
                    IsActive = true,
                    EmailVerified = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await context.Users.AddAsync(adminUser);
            }

            // Seed Sample Categories
            if (!await context.Categories.AnyAsync())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Men's Perfumes", Description = "Exclusive men's fragrances", IsActive = true, CreatedAt = DateTime.UtcNow },
                    new Category { Name = "Women's Perfumes", Description = "Elegant women's fragrances", IsActive = true, CreatedAt = DateTime.UtcNow },
                    new Category { Name = "Unisex Perfumes", Description = "Versatile fragrances for all", IsActive = true, CreatedAt = DateTime.UtcNow },
                    new Category { Name = "Niche Perfumes", Description = "Premium and exclusive fragrances", IsActive = true, CreatedAt = DateTime.UtcNow }
                };

                await context.Categories.AddRangeAsync(categories);
            }

            // Seed Sample Brands
            if (!await context.Brands.AnyAsync())
            {
                var brands = new List<Brand>
                {
                    new Brand { Name = "Chanel", Description = "Luxury French fashion house", IsActive = true, CreatedAt = DateTime.UtcNow },
                    new Brand { Name = "Dior", Description = "French luxury goods company", IsActive = true, CreatedAt = DateTime.UtcNow },
                    new Brand { Name = "Tom Ford", Description = "American luxury fashion house", IsActive = true, CreatedAt = DateTime.UtcNow },
                    new Brand { Name = "Jo Malone", Description = "British luxury fragrance brand", IsActive = true, CreatedAt = DateTime.UtcNow },
                    new Brand { Name = "Creed", Description = "British luxury perfume house", IsActive = true, CreatedAt = DateTime.UtcNow }
                };

                await context.Brands.AddRangeAsync(brands);
            }

            // Seed Sample Products
            if (!await context.Products.AnyAsync())
            {
                var categories = await context.Categories.ToListAsync();
                var brands = await context.Brands.ToListAsync();

                var products = new List<Product>
                {
                    new Product
                    {
                        Name = "Chanel N°5",
                        Description = "The most famous perfume in the world",
                        Price = 150.00m,
                        Stock = 50,
                        BrandId = brands.FirstOrDefault(b => b.Name == "Chanel")?.BrandId,
                        CategoryId = categories.FirstOrDefault(c => c.Name == "Women's Perfumes")?.CategoryId,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Product
                    {
                        Name = "Dior Sauvage",
                        Description = "A powerful freshness, a raw and noble beauty",
                        Price = 120.00m,
                        Stock = 75,
                        BrandId = brands.FirstOrDefault(b => b.Name == "Dior")?.BrandId,
                        CategoryId = categories.FirstOrDefault(c => c.Name == "Men's Perfumes")?.CategoryId,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Product
                    {
                        Name = "Tom Ford Black Orchid",
                        Description = "A mysterious and sensual fragrance",
                        Price = 200.00m,
                        Stock = 30,
                        BrandId = brands.FirstOrDefault(b => b.Name == "Tom Ford")?.BrandId,
                        CategoryId = categories.FirstOrDefault(c => c.Name == "Unisex Perfumes")?.CategoryId,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Product
                    {
                        Name = "Jo Malone Wood Sage & Sea Salt",
                        Description = "A fresh and mineral scent",
                        Price = 85.00m,
                        Stock = 60,
                        BrandId = brands.FirstOrDefault(b => b.Name == "Jo Malone")?.BrandId,
                        CategoryId = categories.FirstOrDefault(c => c.Name == "Unisex Perfumes")?.CategoryId,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Product
                    {
                        Name = "Creed Aventus",
                        Description = "A fruity chypre fragrance",
                        Price = 350.00m,
                        Stock = 25,
                        BrandId = brands.FirstOrDefault(b => b.Name == "Creed")?.BrandId,
                        CategoryId = categories.FirstOrDefault(c => c.Name == "Niche Perfumes")?.CategoryId,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                };

                await context.Products.AddRangeAsync(products);
            }

            // Seed Sample Coupons
            if (!await context.Coupons.AnyAsync())
            {
                var coupons = new List<Coupon>
                {
                    new Coupon
                    {
                        Code = "WELCOME10",
                        DiscountPercent = 10,
                        MinOrderAmount = 50.00m,
                        IsActive = true,
                        ValidUntil = DateTime.UtcNow.AddMonths(3),
                        CreatedAt = DateTime.UtcNow
                    },
                    new Coupon
                    {
                        Code = "SAVE20",
                        DiscountPercent = 20,
                        MinOrderAmount = 100.00m,
                        IsActive = true,
                        ValidUntil = DateTime.UtcNow.AddMonths(6),
                        CreatedAt = DateTime.UtcNow
                    }
                };

                await context.Coupons.AddRangeAsync(coupons);
            }

            await context.SaveChangesAsync();
        }
    }
} 