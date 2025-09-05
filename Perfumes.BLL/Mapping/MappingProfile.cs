using AutoMapper;
using Perfumes.BLL.DTOs.Brand;
using Perfumes.BLL.DTOs.Cart;
using Perfumes.BLL.DTOs.Category;
using Perfumes.BLL.DTOs.Coupon;
using Perfumes.BLL.DTOs.Order;
using Perfumes.BLL.DTOs.Product;
using Perfumes.BLL.DTOs.User;
using Perfumes.DAL.Entities;

namespace Perfumes.BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Category Mappings
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
            CreateMap<Category, CategoryWithProductsDto>();
            CreateMap<Category, CategoryTreeDto>()
                .ForMember(dest => dest.SubCategories, opt => opt.MapFrom(src => src.SubCategories));

            // Brand Mappings
            CreateMap<Brand, BrandDto>().ReverseMap();
            CreateMap<Brand, CreateBrandDto>().ReverseMap();
            CreateMap<Brand, UpdateBrandDto>().ReverseMap();
            CreateMap<Brand, BrandWithProductsDto>().ReverseMap();
            CreateMap<Brand, BrandWithProductCountDto>()
                .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products != null ? src.Products.Count : 0));

            // Product Mappings
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand != null ? src.Brand.Name : null))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));
            CreateMap<CreateProductDto, Product>().ReverseMap();
            // When updating, don't overwrite key and don't map nulls
            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.ProductId, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Product, ProductSummaryDto>().ReverseMap();
            CreateMap<Product, ProductWithReviewsDto>().ReverseMap();

            // Cart Mappings
            CreateMap<Cart, CartItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src => src.Product.ImageUrl))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price));

            // Coupons Mappings
            CreateMap<Coupon, CouponDto>();
            CreateMap<CreateCouponDto, Coupon>();
            CreateMap<UpdateCouponDto, Coupon>();
            CreateMap<Coupon, CouponResultDto>()
                .ForMember(dest => dest.DiscountValue, opt => opt.Ignore())
                .ForMember(dest => dest.IsValid, opt => opt.Ignore())
                .ForMember(dest => dest.IsPercentage, opt => opt.Ignore())
                .ForMember(dest => dest.Message, opt => opt.Ignore());

            // ShippingInfo
            CreateMap<ShippingInfoDto, ShippingInfo>();
            CreateMap<ShippingInfo, ShippingInfoDto>();

            // OrderDetail
            CreateMap<CreateOrderItemDto, OrderDetail>();

            // Order
            CreateMap<Order, OrderSummaryDto>()
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount ?? 0));
            CreateMap<OrderDetail, OrderItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty));
            CreateMap<Order, OrderDetailsDto>()
                .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.SubTotal ?? 0))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount ?? 0))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderDetails))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ShippingInfo != null ? src.ShippingInfo.Email : null))
                .ForMember(dest => dest.ShippingInfo, opt => opt.MapFrom(src => src.ShippingInfo));

            // User Mappings
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.TotalOrders, opt => opt.Ignore())
                .ForMember(dest => dest.TotalReviews, opt => opt.Ignore())
                .ForMember(dest => dest.WishlistItems, opt => opt.Ignore())
                .ForMember(dest => dest.TotalSpent, opt => opt.Ignore());

            CreateMap<UserRegisterDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "Customer"))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.EmailVerified, opt => opt.MapFrom(_ => false))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true));

            CreateMap<UpdateUserDto, User>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<User, UserProfileDto>()
                .ForMember(dest => dest.TotalOrders, opt => opt.Ignore())
                .ForMember(dest => dest.TotalReviews, opt => opt.Ignore())
                .ForMember(dest => dest.WishlistItems, opt => opt.Ignore())
                .ForMember(dest => dest.TotalSpent, opt => opt.Ignore());
        }
    }
}
