using RestaurantManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Areas.Dashboard.ViewModels
{
    public class ItemOrderViewModel
    {
        public Guid OrderId { get; set; }
        public Guid ItemId { get; set; }
        [Required]
        public string ItemName { get; set; } = string.Empty;
        [Required]
        public short Quantity { get; set; }
        [Required]
        public decimal Price { get; set; }
        public ICollection<Order> Orders { get; set; } = [];
        public ICollection<Item> Items { get; set; } = [];
    }
}