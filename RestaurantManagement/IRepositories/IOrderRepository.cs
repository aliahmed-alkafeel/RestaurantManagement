using RestaurantManagement.Models;

namespace RestaurantManagement.IRepositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        public Task<List<Order>> GetAllOrdersWithItemsAsync();
        public Task<Order?> GetOrderWithItemsByIdAsync(Guid id);
    }
}
