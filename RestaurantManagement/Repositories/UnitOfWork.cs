using RestaurantManagement.Data;
using RestaurantManagement.IRepositories;
using RestaurantManagement.Models;

namespace RestaurantManagement.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IEmployeeRepository Employees { get; }
        public IGroupRepository Groups { get; }

        public IRepository<Item> Items { get; }

        public IRepository<Category> Categories { get; }

        public IRepository<Order> Orders { get; }

        public IRepository<Discount> Discounts { get; }

        public IRepository<ItemOrder> ItemOrders { get; }

        public IRoleRepository Roles { get; }

        public IGroupRoleRepository GroupsRoles { get; }
        public UnitOfWork(
            AppDbContext context,
            IEmployeeRepository employees,
            IRepository<Item> items,
            IRepository<Category> categories,
            IRepository<Order> orders,
            IRepository<Discount> discounts,
            IRepository<ItemOrder> itemOrders,
            IRoleRepository roles,
            IGroupRepository groups,
            IGroupRoleRepository groupsRoles)
            {
            _context = context;
            Employees = employees;
            Items = items;
            Categories = categories;
            Orders = orders;
            Discounts = discounts;
            ItemOrders = itemOrders;
            Roles = roles;
            Groups = groups;
            GroupsRoles = groupsRoles;
            }
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync();
        }
    }
}
