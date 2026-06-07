using ECommerceAPI.DTOs;
using ECommerceAPI.Model;

namespace ECommerceAPI.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(int userId, CreateOrderDto orderDto);
        Task<IEnumerable<Order>> GetUserOrdersAsync(int userId);
        Task<Order> GetOrderByIdAsync(int orderId, int userId);
    }

}
