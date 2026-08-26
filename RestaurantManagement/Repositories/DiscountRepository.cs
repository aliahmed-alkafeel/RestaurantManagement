using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Data;
using RestaurantManagement.IRepositories;
using RestaurantManagement.Models;

namespace RestaurantManagement.Repositories
{
    public class DiscountRepository : Repository<Discount>, IDiscountRepository
    {
        public DiscountRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Discount>> GetAllDiscountsWithItemsAsync()
        {
            return await _dbSet.Where(d => !d.IsDeleted).Include(i => i.Items).ToListAsync();
        }

        public async Task<Discount?> GetDiscountWithItemsByIdAsync(Guid id)
        {
            return await _dbSet.Where(d => !d.IsDeleted).Include(i => i.Items).FirstOrDefaultAsync();
        }
    }
}
