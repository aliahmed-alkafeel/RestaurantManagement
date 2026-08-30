//using RestaurantManagement.Data;
//using RestaurantManagement.IRepositories;
//using RestaurantManagement.Models;

//namespace RestaurantManagement.Repositories
//{
//    public class ItemOrderRepository : Repository<ItemOrder>, IItemOrderRepository
//    {
//        public ItemOrderRepository(AppDbContext context) : base(context)
//        {
//        }

//        public void Delete(Item item, Order order, Guid deletedbyId, CancellationToken cancellationToken = default)
//        {
//            item.IsUpdated = true;
//            item.UpdatedAt = DateTime.UtcNow;
//            item.UpdatedById = deletedbyId;
//            _dbSet.Update(new ItemOrder { ItemId = item.Id, OrderId = order.Id});
//        }

//        public void Update(Item item, Order order, Guid updatedById, CancellationToken cancellationToken = default)
//        {
//            item.IsUpdated = true;
//            item.UpdatedAt = DateTime.UtcNow;
//            item.UpdatedById = updatedById;
//            _dbSet.Update(new ItemOrder { ItemId = item.Id, OrderId = order.Id });
//        }
//    }
//}
