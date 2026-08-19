using RestaurantManagement.Models;

namespace RestaurantManagement.IRepositories
{
    public interface IGroupRepository : IRepository<Group>
    {
        Task<Guid> GetIdByNameAsync(UserGroup group);
    }
}
