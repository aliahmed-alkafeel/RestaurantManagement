using Microsoft.AspNetCore.Mvc.Rendering;
using RestaurantManagement.Models;
using System.ComponentModel.DataAnnotations;
using RestaurantManagement.ViewModels;

namespace RestaurantManagement.Areas.Dashboard.ViewModels
{
    public class UpdateEmployeeViewModel : BaseCommand, IValidatableObject
    {
        [Required]
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = null!;
        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EmployeeStartingDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? EmployeeEndingDate { get; set; }
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = null!;

        [Required]
        public Guid Group { get; set; }
        [MaxLength(50)]
        public string? GroupName { get; set; } = null!;
        public List<SelectListItem> Groups = [];

        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The Length must be between {1} and {2}")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Confirm password is not identical to the password")]
        [Length(2, 50, ErrorMessage = "The Length must be between {1} and {2}")]
        public string? ConfirmPassword { get; set; }
        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Password) &&
                string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                if (EmployeeEndingDate <= EmployeeStartingDate)
                {
                    yield return new ValidationResult(
                        "Employee ending date must be after the starting date.",
                        [nameof(EmployeeEndingDate)]);
                }

                yield break;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                yield return new ValidationResult(
                    "Password is required.",
                    [nameof(Password)]);

                yield break;
            }

            if (string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                yield return new ValidationResult(
                    "Confirm password is required.",
                    [nameof(ConfirmPassword)]);

                yield break;
            }

            if (EmployeeEndingDate <= EmployeeStartingDate)
            {
                yield return new ValidationResult(
                    "Employee ending date must be after the starting date.",
                    [nameof(EmployeeEndingDate)]);
            }
        }
    }
}
