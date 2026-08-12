using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Models
{
    public class Item : BaseModel
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public decimal Price { get; set; }
        [Required]
        [MaxLength(50)]
        public string ItemName { get; set; } = null!;
        public bool IsAvailable { get; set; }
        public bool IsActive { get; set; } = true;
        public Category Category { get; set; } = null!;
    }
}
