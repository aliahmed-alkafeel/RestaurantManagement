using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Models
{
    public class Role : BaseSoftDelete
    {
        public Guid RoleId { get; set; }
        [Required]
        public string RoleName { get; set; } = null!;
        public ICollection<ItemOrder> ItemOrders { get; set; } = [];
    }
}