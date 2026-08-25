using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Models
{
    public class ItemOrder : BaseModel
    {
        public Guid OrderId { get; set; }
        public Guid ItemId { get; set; }
        public short Quantity { get; set; }
        public decimal Price { get; set; }
        public ICollection<Order> Order { get; set; } = null!;
        public ICollection<Item> Item { get; set; } = null!;
    }
}