using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Models
{
    public class Order : BaseModel
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public ICollection<ItemOrder> ItemOrders { get; set; } = [];
    }
    public enum OrderStatus
    {
        Confirmed,
        Preparing,
        Pending,
        Ready,
        Completed,
        Cancelled
    }
}
