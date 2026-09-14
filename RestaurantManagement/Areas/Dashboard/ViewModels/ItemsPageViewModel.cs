using RestaurantManagement.Models;
using RestaurantManagement.ViewModels;

namespace RestaurantManagement.Areas.Dashboard.ViewModels
{
    public class ItemsPageViewModel : BaseDto
    {
        public PaginatedList<Item> Items { get; set; } = new();
        public ItemFilterViewModel Filter { get; set; } = new();
        public List<Category> Categories { get; set; } = [];
    }
}
