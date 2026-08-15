using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(100, ErrorMessage = "The Length is more than allowd")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "LastName name is required")]
        [MaxLength(100, ErrorMessage = "The Length is more than allowd")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Username is required")]
        [MaxLength(100, ErrorMessage ="The Length is more than allowd")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "There is an error with this email")]
        [MaxLength(100, ErrorMessage = "The Length is more than allowd")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Length(2, 50, ErrorMessage = "The Length must be between {1} and {2}")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Confirm password is not identical to the password")]
        [Length(2, 50, ErrorMessage = "The Length must be between {1} and {2}")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
