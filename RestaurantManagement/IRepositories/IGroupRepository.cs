using RestaurantManagement.Models;

namespace RestaurantManagement.IRepositories
{
    public interface IGroupRepository : IRepository<Group>
    {
        Task<Guid> GetIdByNameAsync(string group, CancellationToken cancellationToken = default);
        Task<List<Group>> GetAllGroupsWithRolesAsync(CancellationToken cancellationToken = default);
        Task<Group> GetGroupWithRolesByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
