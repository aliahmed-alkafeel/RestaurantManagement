using RestaurantManagement.Models;

namespace RestaurantManagement.IRepositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        public Task<List<Order>> GetAllOrdersWithItemsAsync(CancellationToken cancellationToken = default);
        public Task<Order?> GetOrderWithItemsByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
