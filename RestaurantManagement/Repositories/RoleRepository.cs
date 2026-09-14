using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Data;
using RestaurantManagement.IRepositories;
using RestaurantManagement.Models;

namespace RestaurantManagement.Repositories
{
    public class RoleRepository : Repository<Role>,IRoleRepository
    {
        public RoleRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }
        public async Task<List<Role>> GetRolesByNamesAsync(List<UserRole> rolesNames)
        {
            return await _dbSet.Where(r => rolesNames.Contains(r.RoleName)).ToListAsync();
        }
    }
}
