using RestaurantManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.ViewModels
{
    public class OrderStatusViewModel
    {
        [Required(ErrorMessage = "Order is required.")]
        public Guid OrderId { get; set; }
        [Required(ErrorMessage = "Order status is required.")]
        public int Status { get; set; }
    }
}
