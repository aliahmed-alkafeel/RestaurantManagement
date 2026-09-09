using RestaurantManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Areas.Dashboard.ViewModels
{
    public class DiscountViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Discount percentage is required.")]
        [Range(0, 100)]
        public decimal DiscountPercentage { get; set; }
        [Required(ErrorMessage = "Discount starting date is required.")]
        public DateTime DiscountStartingDate { get; set; }
        public DateTime DiscountEndingDate { get; set; }
        //[Required( ErrorMessage = ("You must Add one item at least."))]
        //[MinLength(1, ErrorMessage = ("You must Add one item at least."))]
        public List<Guid> ItemIds { get; set; } = [];
        //[Required( ErrorMessage = ("You must Add one item at least."))]
        //[MinLength(1, ErrorMessage = ("You must Add one item at least."))]
        public List<Item> Items { get; set; } = [];
    }
}
