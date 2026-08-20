using RestaurantManagement.Models;

namespace RestaurantManagement.IRepositories
{
    public interface IGroupRoleRepository : IRepository<GroupRole>
    {
        Task DeleteByGroupIdAsync(Guid groupId);
    }
}
