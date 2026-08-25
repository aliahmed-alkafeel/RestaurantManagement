using RestaurantManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Areas.Dashboard.ViewModels
{
    public class CategoryViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public CategoryType Type { get; set; }
        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; } = null!;

    }
}
