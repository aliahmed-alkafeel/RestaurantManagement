using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Models
{
    public class Category : BaseModel
    {
        public Guid Id { get; set; }
        public CategoryType Type { get; set; }
        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; } = null!;
        public ICollection<Item> Items { get; set; } = [];
    }
    public enum CategoryType
    {
        Unclassified = 0,
        Eastern = 1,
        Western = 2,
        Seafood = 3,
        Vegetarian = 4,
        Healthy = 5,
        Desserts = 6,
        Soups = 7,
        Beverages = 8
}
}