using RestaurantManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.ViewModels
{
    public class EmployeeViewModel
    {
        public string FirstName { get; set; } = null!;
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = null!;
        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = null!;
        public DateTime EmployeeStartingDate { get; set; }
        public DateTime? EmployeeEndingDate { get; set; }
        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = null!;
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = null!;
        [Required]
        [MaxLength(50)]
        public UserGroup Group { get; set; }

    }
}
