using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        [Required]
        public string OrderStatus { get; set; } = null!;
        public ICollection<ItemOrder> ItemOrders { get; set; } = [];
    }
}
