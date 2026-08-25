using RestaurantManagement.Models;

namespace RestaurantManagement.IRepositories
{
    public interface IOrderRepository
    {
        public Task<List<Order>> GetAllOrdersWithItemsAsync();
    }
}
