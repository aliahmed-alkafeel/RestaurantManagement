using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Data;
using RestaurantManagement.IRepositories;
using RestaurantManagement.Models;

namespace RestaurantManagement.Repositories
{
    public class DiscountRepository : Repository<Discount>, IDiscountRepository
    {
        public DiscountRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public async Task<List<Discount>> GetAllDiscountsWithItemsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.OrderBy(d=>d.DiscountStartingDate).Include(i => i.Items).ToListAsync(cancellationToken);
        }

        public async Task<Discount?> GetDiscountWithItemsByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbSet.Where(d => d.Id == id).Include(i => i.Items).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
