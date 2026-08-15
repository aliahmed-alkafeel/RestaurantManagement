using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.ViewModels
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "Username is required")]
        [MaxLength(100,ErrorMessage ="The Length is more than allowd")]
        public string Username { get; set; } = null!;

        [DataType(DataType.Password)]
        [Length(2, 50,ErrorMessage = "The Length must be between {1} and {2}")]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; }
    }
}
