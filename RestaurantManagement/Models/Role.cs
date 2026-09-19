using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Models
{
    public class Role : BaseModel
    {
        public UserRole RoleName { get; set; }
        public ICollection<ItemOrder> ItemOrders { get; set; } = [];
    }
    public enum UserRole
    {
        Unclassified = 0,
        ManageEmployees,
        AccessEmployees,

        AccessItems,
        ManageItems,

        AccessCategories,
        ManageCategories,

        AccessOrders,
        ManageOrders,

        AccessDiscounts,
        ManageDiscounts,
        AccessGroups,
        ManageGroups,
        AccessDetails,
    }
}