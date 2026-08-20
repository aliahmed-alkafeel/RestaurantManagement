using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Data;
using RestaurantManagement.IRepositories;
using RestaurantManagement.Models;

namespace RestaurantManagement.Repositories
{
    public class GroupRepository : Repository<Group>, IGroupRepository
    {
        public GroupRepository(AppDbContext context) : base(context)
        {
        }



        public async Task<List<Group>> GetAllGroupsWithRolesAsync()
        {
            var groups = await _dbSet.Include(g => g.GroupRoles).ThenInclude(gr => gr.Role).ToListAsync();
            return groups;
        }
        public async Task<Group> GetGroupWithRolesByIdAsync(Guid id)
        {
            var group = await _dbSet.Include(g => g.GroupRoles).ThenInclude(gr => gr.Role).FirstOrDefaultAsync(g => g.Id == id);
            if (group is null) throw new KeyNotFoundException();
            return group;
        }

        public async Task<Guid> GetIdByNameAsync(UserGroup group)
        {
            Console.WriteLine(group);
            var dbGroup = await _dbSet.FirstOrDefaultAsync(g => g.GroupName == group);
            Console.WriteLine(dbGroup);
            Console.WriteLine(dbGroup?.Id);
            if(dbGroup is null)
            {
                throw new KeyNotFoundException($"Group {group.ToString()} is not found");
            }
            return dbGroup.Id;

        }
    }
}
