using ECommerceAPI.DTOs;
using ECommerceAPI.Model;
using ECommerceAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IEmailQueue _emailQueue;
        ILogger<OrdersController> logger;

        public OrdersController(IOrderService orderService, IEmailQueue emailQueue, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _emailQueue = emailQueue;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
        {
            try
            {
                logger.LogInformation("Order Creation started.");
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!int.TryParse(userIdStr, out var userId))
                    return Unauthorized();

                var order = await _orderService.CreateOrderAsync(userId, dto);
                await _emailQueue.QueueEmailAsync(
                    new EmailRequest
                    {
                        To = order.User.Email?? "test@example.com",
                        Subject = "Order Created",
                        Body = $"Your Order #{order.Id} has been created."
                    });
                logger.LogInformation("Order Creation completed successfully. Order ID: {OrderId}", order.Id);

                return CreatedAtAction(nameof(GetOrderById), new { orderId = order.Id }, order);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while creating order.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetUserOrders()
        {
            logger.LogInformation("Retrieving user orders.");
            try
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!int.TryParse(userIdStr, out var userId))
                    return Unauthorized();

                var orders = await _orderService.GetUserOrdersAsync(userId);
                logger.LogInformation("User Orders retrieved. Count: {Count}", orders.Count());
                return Ok(orders);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"Error occurred while retrieving user orders.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            try
            {
                logger.LogInformation("Retrieving order by ID: {OrderId}", orderId);
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!int.TryParse(userIdStr, out var userId))
                    return Unauthorized();

                var order = await _orderService.GetOrderByIdAsync(orderId, userId);
                if (order == null) return NotFound();
                return Ok(order);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving order by ID.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
