using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.ViewModels
{
    public class CreateItemOrderViewModel 
    {
        [Required]
        public Guid ItemId { get; set; }
        [Range(1, 1000, ErrorMessage = "Quantity must be between 0 and 1000.")]
        public short Quantity { get; set; }
    }
}
