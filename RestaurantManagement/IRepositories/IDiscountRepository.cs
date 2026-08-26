using RestaurantManagement.Models;

namespace RestaurantManagement.IRepositories
{
    public interface IDiscountRepository : IRepository<Discount>
    {
        public Task<List<Discount>> GetAllDiscountsWithItemsAsync();
        public Task<Discount?> GetDiscountWithItemsByIdAsync(Guid id);
    }
}