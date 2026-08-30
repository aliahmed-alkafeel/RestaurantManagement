using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Models
{
    public class ItemOrder : BaseModel
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ItemId { get; set; }
        public short Quantity { get; set; }
        public decimal Price { get; set; }
        public Order Order { get; set; } = null!;
        public Item Item { get; set; } = null!;
    }
}