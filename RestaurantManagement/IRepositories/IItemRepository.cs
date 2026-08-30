using RestaurantManagement.Models;

namespace RestaurantManagement.IRepositories
{
    public interface IItemRepository : IRepository<Item>
    {
        public Task<List<Item>> GetItemsWithCategories();
    }
}
