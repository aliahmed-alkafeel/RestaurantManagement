using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Models
{
    public class Group : BaseModel
    {
        public Guid Id { get; set; }
        public string GroupName { get; set; } = null!;
        public ICollection<Employee> Employees { get; set; } = [];
        public ICollection<GroupRole> GroupRoles { get; set; } = [];
    }
    public enum InitUserGroup
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
