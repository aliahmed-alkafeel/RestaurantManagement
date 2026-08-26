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
        IOrderRepository Orders { get; }
        IDiscountRepository Discounts { get; }
        IRepository<ItemOrder> ItemOrders { get; }
        IGroupRoleRepository GroupsRoles { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}