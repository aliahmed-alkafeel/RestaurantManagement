using RestaurantManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Areas.Dashboard.ViewModels
{
    public class OrderViewModel
    {
        public Guid Id { get; set; }
        public int TableId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public ICollection<ItemOrderViewModel> ItemOrders { get; set; } = [];
    }
}
