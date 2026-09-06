using RestaurantManagement.Areas.Dashboard.ViewModels;

namespace RestaurantManagement.ViewModels
{
    public class CreateOrderViewModel
    {
        public int TableId { get; set; }
        public ICollection<CreateItemOrderViewModel> ItemOrders { get; set; } = [];
    }
}
