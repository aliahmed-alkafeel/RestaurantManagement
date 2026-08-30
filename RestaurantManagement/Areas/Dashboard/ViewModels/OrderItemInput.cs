using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Areas.Dashboard.ViewModels
{
    public class OrderItemInput
    {
        public Guid ItemId { get; set; }
        [Required]
        public string ItemName { get; set; } = null!;
        public decimal Price { get; set; }
        public short Quantity { get; set; } = 1;
    }
}
