using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Models
{
    public class Role : BaseModel
    {
        public Guid Id { get; set; }
        public UserRole RoleName { get; set; }
        public ICollection<ItemOrder> ItemOrders { get; set; } = [];
    }
    public enum UserRole
    {
        Unclassified = 0,

        AccessDashboard,

        AccessEmployees,
        ManageEmployees,

        AccessUsers,
        ManageUsers,

        AccessItems,
        ManageItems,

        AccessCategories,
        ManageCategories,

        AccessOrders,
        ManageOrders,

        AccessDiscounts,
        ManageDiscounts,

        AccessReports,
        ExportReports,

        AccessPayments,
        ManagePayments,

        AccessSettings,
        ManageSettings
    }
}