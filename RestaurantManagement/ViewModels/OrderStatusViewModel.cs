using RestaurantManagement.Models;

namespace RestaurantManagement.ViewModels
{
    public class OrderStatusViewModel
    {
        public Guid OrderId { get; set; }
        public OrderStatus Status { get; set; }
    }
}
