using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Models
{
    public class Employee : BaseSoftDelete
    {
        public Guid Id { get; set; }
        public Guid GroupId { get; set; }
        [Required]
        [MaxLength(50)]
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
        public string HashedPassword { get; set; } = null!;
        public Group? Group { get; set; }
    }
}
