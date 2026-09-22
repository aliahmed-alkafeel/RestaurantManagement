using RestaurantManagement.Models;

namespace RestaurantManagement.IRepositories
{
    public interface IUnitOfWork
    {
        IRepository<Employee> Employees { get; }
        IRepository<Role> Roles { get; }
        IRepository<Group> Groups { get; }
        IRepository<Item> Items { get; }
        IRepository<Category> Categories { get; }
        IRepository<Order> Orders { get; }
        IRepository<Discount> Discounts { get; }
        IRepository<ItemOrder> ItemOrders { get; }
        IRepository<GroupRole> GroupsRoles { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}