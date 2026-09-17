using RestaurantManagement.Models;
using System.ComponentModel.DataAnnotations;
using RestaurantManagement.ViewModels;

namespace RestaurantManagement.Areas.Dashboard.ViewModels
{
    public class DiscountViewModel : BaseCommand, IValidatableObject
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Discount percentage is required.")]
        [Range(0, 100,ErrorMessage ="Discount percentage must between 0 and 100.")]
        public decimal DiscountPercentage { get; set; }
        [Required(ErrorMessage = "Discount starting date is required.")]
        public DateTime DiscountStartingDate { get; set; } = DateTime.Today;
        public DateTime DiscountEndingDate { get; set; } = DateTime.Today.AddDays(1).AddSeconds(-1);
        [MinLength(1, ErrorMessage = ("You must Add one item at least."))]
        public List<Guid> ItemIds { get; set; } = [];
        public List<Item> Items { get; set; } = [];
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DiscountEndingDate <= DiscountStartingDate)
        {
                yield return new ValidationResult("Discount ending date must be after the starting date.",
                     [nameof(DiscountEndingDate)] );
        }
    }

    }

}