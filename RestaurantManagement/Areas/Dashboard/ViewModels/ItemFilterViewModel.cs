using RestaurantManagement.Models;
using System.ComponentModel.DataAnnotations;
using RestaurantManagement.ViewModels;

namespace RestaurantManagement.Areas.Dashboard.ViewModels
{
    public class ItemFilterViewModel : BaseQuery
    {
        public string? Search { get; set; }
        public CategoryType? Type { get; set; }
        public Guid? CategoryId { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Minimum price cannot be negative.")]
        public decimal? MinPrice { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Maximum price cannot be negative.")]
        public decimal? MaxPrice { get; set; }
        public bool? IsAvailable { get; set; }
        public bool? IsValid { get; set; }
        public string? Sort { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0.")]
        public int Page { get; set; } = 1;
        [Range(1, 1000, ErrorMessage = "Page size must be between 1 and 1000.")]
        public int PageSize { get; set; } = 10;
    }
}
