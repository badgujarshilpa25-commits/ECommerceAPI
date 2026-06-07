using EcommerceAPI.Data;
using ECommerceAPI.DTOs;
using ECommerceAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        ILogger<OrderService> _logger;
        public OrderService(AppDbContext context, ILogger<OrderService> logger)
        { 
            _context = context;
            _logger = logger;
        }
        public async Task<Order> CreateOrderAsync(int userId, CreateOrderDto orderDto)
        {
            try
            {
                var order = new Order
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    Status = "Pending"
                };

                decimal totalAmount = 0m;

                foreach (var item in orderDto.Items)
                {
                    var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
                    if (product == null)
                    {
                        throw new InvalidOperationException($"Product with id {item.ProductId} not found.");
                    }

                    var unitPrice = product.Price;

                    var orderItem = new OrderItem
                    {
                        ProductId = product.Id,
                        Quantity = item.Quantity,
                        UnitPrice = unitPrice
                    };

                    order.OrderItems.Add(orderItem);

                    totalAmount += unitPrice * item.Quantity;
                }

                order.TotalAmount = totalAmount;

                _context.Orders.Add(order);

                // Save order and all order items in one call
                await _context.SaveChangesAsync();

                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating order.");
                throw;
            }
        }

        public async Task<Order> GetOrderByIdAsync(int orderId, int userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId) 
                ?? throw new InvalidOperationException("Order not found.");
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(int userId)
        {
            return await _context.Orders
                .Include(o=>o.OrderItems)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }
    }
}
