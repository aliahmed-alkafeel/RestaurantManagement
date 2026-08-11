using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Models
{
    public class Group
    {
        public Guid Id { get; set; }
        [Required]
        public string GroupName { get; set; } = null!;
        public ICollection<Employee> Employees { get; set; } = [];
        public ICollection<GroupRole> GroupRoles { get; set; } = [];
    }
}
