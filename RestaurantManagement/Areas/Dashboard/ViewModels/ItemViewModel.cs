using RestaurantManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Areas.Dashboard.ViewModels
{
    public class ItemViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Category is required.")]
        public Guid CategoryId { get; set; }
        [Required(ErrorMessage ="Price is required.")]
        public decimal Price { get; set; }
        public decimal? DiscountPercentage { get; set; }
        [Required]
        [MaxLength(50)]
        public string ItemName { get; set; } = null!;
        public string ImageUrl { get; set; } = "~/images/items/default.jpg";
        public IFormFile? ItemImage { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsActive { get; set; } 
        public Category? Category { get; set; } = null!;
    }
}
