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

        public IItemRepository Items { get; }

        public IRepository<Category> Categories { get; }

        public IOrderRepository Orders { get; }

        public IDiscountRepository Discounts { get; }

        public IRepository<ItemOrder> ItemOrders { get; }

        public IRoleRepository Roles { get; }

        public IGroupRoleRepository GroupsRoles { get; }
        public UnitOfWork(
            AppDbContext context,
            IEmployeeRepository employees,
            IItemRepository items,
            IRepository<Category> categories,
            IOrderRepository orders,
            IDiscountRepository discounts,
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
