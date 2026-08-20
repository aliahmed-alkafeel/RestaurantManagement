using RestaurantManagement.Models;

namespace RestaurantManagement.IRepositories
{
    public interface IUnitOfWork
    {
        IEmployeeRepository Employees { get; }
        IRoleRepository Roles { get; }
        IGroupRepository Groups { get; }
        IRepository<Item> Items { get; }
        IRepository<Category> Categories { get; }
        IRepository<Order> Orders { get; }
        IRepository<Discount> Discounts { get; }
        IRepository<ItemOrder> ItemOrders { get; }
        IGroupRoleRepository GroupsRoles { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}