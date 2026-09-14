
using RestaurantManagement.ViewModels;

namespace RestaurantManagement.Areas.Dashboard.ViewModels
{
    public class ItemByCategoryViewModel : BaseDto
    {
        public Guid Id { get; set; }
        public string ItemName { get; set; } = null!;
        public decimal Price { get; set; }
        public string Image { get; set; } = null!;
        public decimal? DiscountPercentage { get; set; }
    }
}
