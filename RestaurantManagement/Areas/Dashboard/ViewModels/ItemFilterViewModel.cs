using RestaurantManagement.Models;

namespace RestaurantManagement.Areas.Dashboard.ViewModels
{
    public class ItemFilterViewModel
    {
        public string? Search { get; set; }
        public CategoryType? Type { get; set; }
        public Guid? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? IsAvailable { get; set; }
        public bool? IsValid { get; set; }
        public string? Sort { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
