using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Models
{
    public class Discount : BaseModel
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        [Range(0,100)]
        public decimal DiscountPercentage { get; set; }
        public DateTime DiscountStartingDate { get; set; }
        public DateTime DiscountEndingDate { get; set; }
        public Item Item { get; set; } = null!;
    }
}
