using Microsoft.AspNetCore.Mvc.Rendering;
using RestaurantManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Areas.Dashboard.ViewModels
{
    public class ManageEmployeeViewModel
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
        public string Group { get; set; } = null!;
        public List<SelectListItem> Groups = [];

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The Length must be between {1} and {2}")]
        public string Password { get; set; } = null!;


        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Confirm password is not identical to the password")]
        [Length(2, 50, ErrorMessage = "The Length must be between {1} and {2}")]
        public string ConfirmPassword { get; set; } = null!;

        

    }
}
