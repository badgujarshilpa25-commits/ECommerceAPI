using ECommerceAPI.DTOs;

namespace ECommerceAPI.Model
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Status { get; set; }

        // Navigation Properties
        public User User { get; set; } = null!;

        public ICollection<OrderItem> OrderItems { get; set; }
            = new List<OrderItem>();

    }
}
