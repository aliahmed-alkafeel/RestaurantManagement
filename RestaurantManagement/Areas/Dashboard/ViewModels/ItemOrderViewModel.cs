using RestaurantManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Areas.Dashboard.ViewModels
{
    public class ItemOrderViewModel
    {
        public Guid OrderId { get; set; }
        public Guid ItemId { get; set; }
        [Required(ErrorMessage = "Item name is required.")]
        public string ItemName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1,10000,ErrorMessage = "Quantity must be between 1 and 10000")]
        public short Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPercentage { get; set; }
        //public Order Orders { get; set; } = null!;
        //public Item Item { get; set; } = null!;
    }
}
