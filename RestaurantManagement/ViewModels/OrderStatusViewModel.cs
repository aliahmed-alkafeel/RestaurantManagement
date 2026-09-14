using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.ViewModels
{
    public class OrderStatusViewModel: BaseCommand
    {
        [Required(ErrorMessage = "Order is required.")]
        public Guid OrderId { get; set; }
        [Required(ErrorMessage = "Order status is required.")]
        public OrderStatus Status { get; set; }
    }
}
