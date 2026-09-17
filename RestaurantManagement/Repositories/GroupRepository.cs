using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Data;
using RestaurantManagement.IRepositories;
using RestaurantManagement.Models;

namespace RestaurantManagement.Repositories
{
    public class GroupRepository : Repository<Group>, IGroupRepository
    {
        public GroupRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public async Task<List<Group>> GetAllGroupsWithRolesAsync(CancellationToken cancellationToken = default)
        {
            var groups = await _dbSet.Include(g => g.GroupRoles).ThenInclude(gr => gr.Role).ToListAsync(cancellationToken);
            return groups;
        }
        public async Task<Group> GetGroupWithRolesByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var group = await _dbSet.Include(g => g.GroupRoles).ThenInclude(gr => gr.Role).FirstOrDefaultAsync(g => g.Id == id,cancellationToken);
            if (group is null) throw new KeyNotFoundException();
            return group;
        }

        public async Task<Guid> GetIdByNameAsync(string group, CancellationToken cancellationToken = default)
        {
            var dbGroup = await _dbSet.FirstOrDefaultAsync(g => g.GroupName == group,cancellationToken);

            if(dbGroup is null)
            {
                throw new KeyNotFoundException($"Group {group.ToString()} is not found");
            }
            return dbGroup.Id;

        }
    }
}
