using RestaurantManagement.Areas.Dashboard.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.ViewModels
{
    public class CreateOrderViewModel
    {
        [Range(1, 1000, ErrorMessage = "You must choose a table.")]
        public int TableId { get; set; }
        [MinLength(1, ErrorMessage = "You must add at least one item.")]
        public ICollection<CreateItemOrderViewModel> ItemOrders { get; set; } = [];
    }
}
