using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.ViewModels
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "Username is required")]
        [MaxLength(100,ErrorMessage ="The Length is more than allowd")]
        public string Username { get; set; } = null!;
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(50,MinimumLength =2,ErrorMessage="The Length must be between {1} and {2}")]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; } = true;
    }
}
