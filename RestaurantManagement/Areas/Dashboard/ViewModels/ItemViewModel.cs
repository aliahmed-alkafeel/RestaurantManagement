using RestaurantManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Areas.Dashboard.ViewModels
{
    public class ItemViewModel
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public decimal Price { get; set; }
        [Required]
        [MaxLength(50)]
        public string ItemName { get; set; } = null!;
        public string ImageUrl { get; set; } = "~/images/items/default.jpg";
        [DataType(DataType.Upload)]
        public IFormFile? Image { get; set; } = null!;
        public bool IsAvailable { get; set; }
        public bool IsActive { get; set; } = true;
        public Category? Category { get; set; } = null!;
    }
}
