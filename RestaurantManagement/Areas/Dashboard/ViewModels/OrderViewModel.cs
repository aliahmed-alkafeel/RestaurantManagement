using RestaurantManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Areas.Dashboard.ViewModels
{
    public class OrderViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage ="You must choice a table.")]
        [Range(1, int.MaxValue, ErrorMessage = "You must choose a table.")]
        public int TableId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        [MinLength(1, ErrorMessage = "You must add at least one item.")]
        public ICollection<ItemOrderViewModel> ItemOrders { get; set; } = [];
    }
}
