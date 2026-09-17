using RestaurantManagement.Data;
using RestaurantManagement.IRepositories;
using RestaurantManagement.Models;
using Microsoft.EntityFrameworkCore;
namespace RestaurantManagement.Repositories
{
    public class GroupRoleRepository : Repository<GroupRole>, IGroupRoleRepository
    {
        public GroupRoleRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }
        public async Task DeleteByGroupIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var groupRoles = await _dbSet.Where(g => g.GroupId == id).ToListAsync(cancellationToken);
            _dbSet.RemoveRange(groupRoles);
        }
    }
}