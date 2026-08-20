using RestaurantManagement.Data;
using RestaurantManagement.IRepositories;
using RestaurantManagement.Models;
using Microsoft.EntityFrameworkCore;
namespace RestaurantManagement.Repositories
{
    public class GroupRoleRepository : Repository<GroupRole>, IGroupRoleRepository
    {
        public GroupRoleRepository(AppDbContext context) : base(context)
        {
        }
        public async Task DeleteByGroupIdAsync(Guid id)
        {
            var groupRoles = await _dbSet.Where(g => g.GroupId == id).ToListAsync();
            _dbSet.RemoveRange(groupRoles);
        }
    }
}
