using RestaurantManagement.Models;
using System.ComponentModel.DataAnnotations;
using RestaurantManagement.ViewModels;

namespace RestaurantManagement.Areas.Dashboard.ViewModels
{
    public class GroupViewModel : BaseCommand
    {
        [Required]
        public Guid Id { get; set; }
        [Required(ErrorMessage = ("Group name is required."))]
        [MaxLength(50)]
        public string GroupName { get; set; } = null!;
        public ICollection<GroupRole> GroupRoles { get; set; } = [];
        public List<UserRole> Roles { get; set; } = [];
    }
}
