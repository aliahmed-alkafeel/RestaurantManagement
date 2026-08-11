using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Models
{
    public class ItemOrder
    {
        public Guid OrderId { get; set; }
        public Guid ItemId { get; set; }
        [Required]
        public short Quantity { get; set; }
        [Required]
        public decimal Price { get; set; }
        public ICollection<Order> Orders { get; set; } = [];
        public ICollection<Item> Items { get; set; } = [];
    }
}