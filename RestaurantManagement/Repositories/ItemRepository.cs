using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Data;
using RestaurantManagement.IRepositories;
using RestaurantManagement.Models;

namespace RestaurantManagement.Repositories
{
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        public ItemRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Item>> GetItemsWithCategories()
        {
            return await _dbSet.Where(i => !i.IsDeleted).Include(i => i.Category).ToListAsync();
        }
    }
}
