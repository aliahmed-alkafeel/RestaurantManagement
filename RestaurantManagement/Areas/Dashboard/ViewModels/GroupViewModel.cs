using RestaurantManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Areas.Dashboard.ViewModels
{
    public class GroupViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public UserGroup GroupName { get; set; }
        public ICollection<GroupRole> GroupRoles { get; set; } = [];
    }
}
