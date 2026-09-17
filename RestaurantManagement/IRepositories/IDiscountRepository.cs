using RestaurantManagement.Models;

namespace RestaurantManagement.IRepositories
{
    public interface IDiscountRepository : IRepository<Discount>
    {
        public Task<List<Discount>> GetAllDiscountsWithItemsAsync(CancellationToken cancellationToken = default);
        public Task<Discount?> GetDiscountWithItemsByIdAsync(Guid id,CancellationToken cancellationToken);
    }
}