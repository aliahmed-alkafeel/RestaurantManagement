using RestaurantManagement.Areas.Dashboard.ViewModels;

namespace RestaurantManagement.ViewModels
{
    public class POSOrdersViewModel : BaseDto
    {
        public List<OrderViewModel> Orders { get; set; } = [];

        public POSOrdersFilterViewModel Filter { get; set; } = new();

        public int TotalCount { get; set; }
    }
}