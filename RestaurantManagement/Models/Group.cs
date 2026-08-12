using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Models
{
    public class Group : BaseModel
    {
        public Guid Id { get; set; }
        public UserGroup GroupName { get; set; }
        public ICollection<Employee> Employees { get; set; } = [];
        public ICollection<GroupRole> GroupRoles { get; set; } = [];
    }
    public enum UserGroup
    {
        Unclassified = 0,
        Administrator,
        Manager,
        Cashier,
        Waiter,
        Chef,
        InventoryManager,
        Accountant,
        Receptionist
    }
}
