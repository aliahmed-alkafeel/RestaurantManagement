using RestaurantManagement.Models;

namespace RestaurantManagement.IRepositories
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<List<Role>> GetRolesByNamesAsync(List<UserRole> rolesNames);
    }
}
