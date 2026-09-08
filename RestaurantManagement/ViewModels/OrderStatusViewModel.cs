using RestaurantManagement.Models;

namespace RestaurantManagement.ViewModels
{
    public class OrderStatusViewModel
    {
        public Guid OrderId { get; set; }
        public int Status { get; set; }
    }
}
