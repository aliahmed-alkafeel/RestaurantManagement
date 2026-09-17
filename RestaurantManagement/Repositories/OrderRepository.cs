using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Data;
using RestaurantManagement.IRepositories;
using RestaurantManagement.Models;
using System.Reflection.Metadata.Ecma335;

namespace RestaurantManagement.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }
        public async Task<List<Order>> GetAllOrdersWithItemsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.Include(o => o.ItemOrders).ThenInclude(io => io.Item).ThenInclude(i => i.Discount).ToListAsync(cancellationToken);
        }
        public async Task<Order?> GetOrderWithItemsByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(o => o.Id == id).Include(o => o.ItemOrders).ThenInclude(io => io.Item).ThenInclude(i => i.Discount).FirstOrDefaultAsync(cancellationToken);
        }

    }
}
