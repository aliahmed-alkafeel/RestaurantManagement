using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Models
{
    public class Order : BaseModel
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        [Required]
        public string OrderStatus { get; set; } = null!;
        public ICollection<ItemOrder> ItemOrders { get; set; } = [];
    }
}
