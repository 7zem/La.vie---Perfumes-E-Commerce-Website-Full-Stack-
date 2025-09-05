using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Perfumes.BLL.DTOs.Order;
using Perfumes.BLL.DTOs.Payment;
using Perfumes.BLL.DTOs.Payments;
using Perfumes.BLL.Services.Interfaces;
using Perfumes.DAL;
using Perfumes.DAL.UnitOfWork;

namespace Perfumes.BLL.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PerfumesDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;
        private readonly IEmailService _emailService;

        public OrderService(IUnitOfWork unitOfWork, PerfumesDbContext dbContext, IMapper mapper, IPaymentService paymentService, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
            _mapper = mapper;
            _paymentService = paymentService;
            _emailService = emailService;
        }

        public async Task<string> CreateOrderAsync(CreateOrderDto dto)
        {
            // Use EF execution strategy to support transient retries with transactions
            var strategy = _dbContext.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () => await CreateOrderInternalAsync(dto));
        }

        private async Task<string> CreateOrderInternalAsync(CreateOrderDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                if (dto.Items == null || dto.Items.Count == 0)
                    throw new InvalidOperationException("Cart is empty.");

                decimal subTotal = 0;
                var orderDetails = new List<Perfumes.DAL.Entities.OrderDetail>();

                foreach (var item in dto.Items)
                {
                    var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
                    if (product == null)
                        throw new InvalidOperationException($"Product with id {item.ProductId} was not found.");

                    var detail = new Perfumes.DAL.Entities.OrderDetail
                    {
                        ProductId = product.ProductId,
                        Quantity = item.Quantity,
                        Price = product.Price,
                        Discount = 0
                    };
                    subTotal += product.Price * item.Quantity;
                    orderDetails.Add(detail);
                }

                decimal discountAmount = 0;
                Perfumes.DAL.Entities.Coupon? coupon = null;

                if (!string.IsNullOrEmpty(dto.CouponCode))
                {
                    coupon = await _unitOfWork.Coupons.FirstOrDefaultAsync(c => c.Code == dto.CouponCode && c.IsActive);
                    if (coupon != null)
                    {
                        bool isValid = true;

                        if (coupon.ValidUntil.HasValue && coupon.ValidUntil < DateTime.UtcNow)
                            isValid = false;

                        if (coupon.MinOrderAmount.HasValue && subTotal < coupon.MinOrderAmount.Value)
                            isValid = false;

                        if (coupon.MaxUses.HasValue && coupon.UsedCount >= coupon.MaxUses.Value)
                            isValid = false;

                        if (!isValid)
                            throw new Exception("Coupon is not valid.");

                        if (coupon.DiscountAmount.HasValue)
                            discountAmount = coupon.DiscountAmount.Value;
                        else if (coupon.DiscountPercent.HasValue)
                            discountAmount = subTotal * (coupon.DiscountPercent.Value / 100);

                        coupon.UsedCount++;
                        await _unitOfWork.Coupons.UpdateAsync(coupon);
                    }
                }

                var order = new Perfumes.DAL.Entities.Order
                {
                    UserId = dto.UserId,
                    OrderNumber = $"ORD-{DateTime.UtcNow.Ticks}",
                    SubTotal = subTotal,
                    DiscountAmount = discountAmount,
                    ShippingCost = 50,
                    TaxAmount = 0,
                    TotalAmount = subTotal - discountAmount + 50,
                    PaymentMethod = dto.PaymentMethod,
                    CouponId = coupon?.CouponId,
                    Notes = dto.Notes,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Orders.AddAsync(order);
                await _unitOfWork.SaveChangesAsync();

                // Add order details
                foreach (var detail in orderDetails)
                {
                    detail.OrderId = order.OrderId;
                    await _unitOfWork.OrderDetails.AddAsync(detail);
                }

                // Add shipping info
                if (dto.ShippingInfo != null)
                {
                    var shippingInfo = _mapper.Map<Perfumes.DAL.Entities.ShippingInfo>(dto.ShippingInfo);
                    shippingInfo.OrderId = order.OrderId;
                    await _unitOfWork.ShippingInfo.AddAsync(shippingInfo);
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                // If payment method is Paymob, initiate payment and return redirect URL
                if (!string.IsNullOrWhiteSpace(dto.PaymentMethod) && dto.PaymentMethod.Equals("Paymob", StringComparison.OrdinalIgnoreCase))
                {
                    var paymentRequest = new PaymentRequestDto
                    {
                        OrderId = order.OrderId,
                        Amount = order.TotalAmount ?? 0,
                        Currency = "EGP",
                        Email = dto.ShippingInfo?.Email ?? "",
                        FullName = $"{dto.ShippingInfo?.FirstName ?? ""} {dto.ShippingInfo?.LastName ?? ""}".Trim(),
                        Phone = dto.ShippingInfo?.PhoneNumber ?? ""
                    };

                    var paymentResponse = await _paymentService.InitiatePaymentAsync(paymentRequest);
                    return paymentResponse.PaymentUrl ?? string.Empty;
                }

                // COD or other: return order number
                return order.OrderNumber;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<OrderDetailsDto?> GetOrderByIdAsync(int orderId)
        {
            var order = await _unitOfWork.Orders.GetOrderWithDetailsAsync(orderId);
            return _mapper.Map<OrderDetailsDto>(order);
        }

        public async Task<OrderDetailsDto?> GetOrderByNumberAsync(string orderNumber)
        {
            var order = await _unitOfWork.Orders.GetOrderByOrderNumberAsync(orderNumber);
            return _mapper.Map<OrderDetailsDto>(order);
        }

        public async Task<IEnumerable<OrderSummaryDto>> GetOrdersByUserAsync(int userId)
        {
            var orders = await _unitOfWork.Orders.GetOrdersByUserAsync(userId);
            return _mapper.Map<IEnumerable<OrderSummaryDto>>(orders);
        }

        public async Task<IEnumerable<OrderSummaryDto>> GetOrdersByStatusAsync(string status)
        {
            var orders = await _unitOfWork.Orders.GetOrdersByStatusAsync(status);
            return _mapper.Map<IEnumerable<OrderSummaryDto>>(orders);
        }

        public async Task<IEnumerable<OrderSummaryDto>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var orders = await _unitOfWork.Orders.GetOrdersByDateRangeAsync(startDate, endDate);
            return _mapper.Map<IEnumerable<OrderSummaryDto>>(orders);
        }

        public async Task<IEnumerable<OrderSummaryDto>> GetPendingOrdersAsync()
        {
            var orders = await _unitOfWork.Orders.GetPendingOrdersAsync();
            return _mapper.Map<IEnumerable<OrderSummaryDto>>(orders);
        }

        public async Task<IEnumerable<OrderSummaryDto>> GetCompletedOrdersAsync()
        {
            var orders = await _unitOfWork.Orders.GetCompletedOrdersAsync();
            return _mapper.Map<IEnumerable<OrderSummaryDto>>(orders);
        }

        public async Task<decimal> GetTotalSalesAsync(DateTime? start = null, DateTime? end = null)
        {
            return await _unitOfWork.Orders.GetTotalSalesAsync(start, end);
        }

        public async Task<int> GetOrdersCountAsync(DateTime? start = null, DateTime? end = null)
        {
            return await _unitOfWork.Orders.GetOrdersCountAsync(start, end);
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null) return false;

            order.Status = newStatus;
            order.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Orders.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();

            // Send email notification
            if (newStatus == "Completed")
            {
                var user = await _unitOfWork.Users.GetByIdAsync(order.UserId ?? 0);
                if (user != null)
                {
                    await _emailService.SendEmailAsync(
                        user.Email,
                        "Order Completed",
                        $"Your order {order.OrderNumber} has been completed successfully."
                    );
                }
            }

            return true;
        }

        public async Task<bool> UpdateShippingInfoAsync(int orderId, ShippingInfoDto dto)
        {
            var shippingInfo = await _unitOfWork.ShippingInfo.FirstOrDefaultAsync(s => s.OrderId == orderId);
            if (shippingInfo == null) return false;

            _mapper.Map(dto, shippingInfo);
            await _unitOfWork.ShippingInfo.UpdateAsync(shippingInfo);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> HandlePaymentCallbackAsync(PaymentCallbackDto dto)
        {
            var order = await _unitOfWork.Orders.GetOrderByOrderNumberAsync(dto.OrderNumber);
            if (order == null) return false;

            if (dto.Status == "Completed")
            {
                order.Status = "Paid";
            }
            else
            {
                order.Status = "PaymentFailed";
            }

            order.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.Orders.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
