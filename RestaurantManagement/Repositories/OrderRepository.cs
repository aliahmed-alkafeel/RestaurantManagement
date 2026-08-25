using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Data;
using RestaurantManagement.IRepositories;
using RestaurantManagement.Models;
using System.Reflection.Metadata.Ecma335;

namespace RestaurantManagement.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<List<Order>> GetAllOrdersWithItemsAsync()
        {
            return await _dbSet.Where(o => !o.IsDeleted).Include(o => o.ItemOrders).ThenInclude(io => io.Items).ThenInclude(i => i.Discount).ToListAsync();
        }
    }
}
