using RestaurantManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Areas.Dashboard.ViewModels
{
    public class GroupViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string GroupName { get; set; } = null!;
        public ICollection<GroupRole> GroupRoles { get; set; } = [];
        public List<UserRole> Roles { get; set; } = [];
    }
}
