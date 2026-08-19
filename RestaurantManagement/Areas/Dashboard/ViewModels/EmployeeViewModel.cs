using RestaurantManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Areas.Dashboard.ViewModels
{
    public class EmployeeViewModel
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
        public UserGroup Group { get; set; }

    }
}
