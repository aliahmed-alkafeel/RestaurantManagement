using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Areas.Dashboard.IServices;
using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.IRepositories;
using RestaurantManagement.Migrations;
using RestaurantManagement.Models;
using RestaurantManagement.Repositories;
using RestaurantManagement.ViewModels;

namespace RestaurantManagement.Areas.Dashboard.Services
{
    public class OrdersService(IUnitOfWork unitOfWork) : IOrdersService
    {
        public async Task<bool> DeleteOrderAsync(Guid modelId)
        {
            var order = await unitOfWork.Orders.GetOrderWithItemsByIdAsync(modelId);
            if (order is null) throw new InvalidOperationException("There is no such order");
            unitOfWork.Orders.Delete(order);
            foreach(ItemOrder itemOrder in order.ItemOrders)
            {
            unitOfWork.ItemOrders.Delete(itemOrder);
            }
            await unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<List<OrderViewModel>> GetAllOrdersAsync()
        {
            var orders = await unitOfWork.Orders.NoTrackingSelect().OrderByDescending(o=>o.OrderDate)
                .Include(o => o.ItemOrders).ThenInclude(io => io.Item).ThenInclude(i => i.Discount).ToListAsync();
            List<OrderViewModel> ordersVm = [];
            foreach (Order order in orders)
            {
                ordersVm.Add(new OrderViewModel
                {
                    Id = order.Id,
                    TableId = order.TableId,
                    OrderDate = order.OrderDate,
                    OrderStatus = order.OrderStatus,
                    TotalPrice = order.TotalPrice
                });
            }
            return ordersVm;
        }

        public async Task<OrderViewModel> GetOrderByIdAsync(Guid id)
        {
            var order = await unitOfWork.Orders.GetOrderWithItemsByIdAsync(id);
            if (order is null) throw new KeyNotFoundException("There is no such order");
            OrderViewModel orderVm = new()
            {
                Id = order.Id,
                TableId = order.TableId,
                OrderStatus = order.OrderStatus,
                TotalPrice = order.TotalPrice,
                OrderDate = order.OrderDate,

                ItemOrders = order.ItemOrders.Select(x => new ItemOrderViewModel
                {
                    ItemId = x.ItemId,
                    ItemName = x.Item.ItemName,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    DiscountPercentage = x.Item.Discount != null &&
                 x.Item.Discount.DiscountStartingDate <= DateTime.UtcNow &&
                 x.Item.Discount.DiscountEndingDate >= DateTime.UtcNow ?
                 x.Item.Price * (1 - (x.Item.Discount.DiscountPercentage / 100)) : x.Item.Price
                }).ToList()
            };
            return orderVm;
        }
        public async Task<bool> CreateOrderAsync(CreateOrderViewModel model)
        {
            if (model is null || model.ItemOrders is null) throw new ArgumentNullException();
            if (!model.ItemOrders.Any()) return false;
            var order = new Order
            {
                Id = Guid.NewGuid(),
                OrderStatus = OrderStatus.Pending,
                TableId = model.TableId,
                OrderDate = DateTime.UtcNow
            };
            await unitOfWork.Orders.AddAsync(order);
            decimal newItemsTotal = 0;

            var items = await unitOfWork.Items.Select().Include(i => i.Category).Include(i => i.Discount).ToListAsync();
            foreach (var newItem in model.ItemOrders)
            {
                if (newItem.Quantity == 0) continue;
                var item = items.FirstOrDefault(x => x.Id == newItem.ItemId);
                if (item is null) throw new ArgumentNullException("One of items is not exists");

                if (!item.IsAvailable || !item.IsActive)
                    throw new InvalidOperationException(
                        $"Item '{item.ItemName}' is not available");

                decimal? hasDiscount = item.Discount != null && item.Discount.DiscountStartingDate <= DateTime.UtcNow && item.Discount.DiscountEndingDate >= DateTime.UtcNow
                        ? item.Discount.DiscountPercentage : null;
                var finalPrice = hasDiscount != null ? item.Price * (1 - (item.Discount!.DiscountPercentage / 100)) : item.Price;
                    var itemOrder = new ItemOrder
                    {
                        ItemId = item.Id,
                        OrderId = order.Id,
                        Quantity = newItem.Quantity,
                        Price = finalPrice,
                    };
                    await unitOfWork.ItemOrders.AddAsync(itemOrder);
                
                newItemsTotal += finalPrice * newItem.Quantity;
            }
            order.TotalPrice = newItemsTotal;
            await unitOfWork.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateOrderAsync(OrderViewModel model)
        {
            if (model is null || model.ItemOrders is null) throw new ArgumentNullException();
            var order = await unitOfWork.Orders.GetOrderWithItemsByIdAsync(model.Id);
            if (order is null) return false;
            order.OrderStatus = model.OrderStatus;
            order.TableId = model.TableId;
            order.OrderDate = model.OrderDate.AddHours(-3);
            var existingItemOrders = order.ItemOrders.ToList();
            var itemIds = model.ItemOrders.Select(x => x.ItemId).ToList();
            decimal newItemsTotal = 0;

            var toRemove = existingItemOrders.Where(io => !itemIds.Contains(io.ItemId));
            
            foreach(var oldItemOrder in toRemove)
            {
                unitOfWork.ItemOrders.Delete(oldItemOrder);
            }
            
            foreach(var newItem in model.ItemOrders)
                {
                    var item = await unitOfWork.Items.GetByIdAsync(newItem.ItemId);
                    if(item is null) throw new ArgumentNullException("One of items is not exists");

                    if (!item.IsAvailable || !item.IsActive)
                        throw new InvalidOperationException(
                            $"Item '{item.ItemName}' is not available");
                decimal finalPrice = item.Discount != null &&
                 item.Discount.DiscountStartingDate <= DateTime.UtcNow &&
                 item.Discount.DiscountEndingDate >= DateTime.UtcNow ?
                 item.Price * (1 - (item.Discount.DiscountPercentage / 100)) : item.Price;

                var existing = existingItemOrders.FirstOrDefault(io => io.ItemId == newItem.ItemId);
                if (existing is not null)
                {
                    existing.IsDeleted = false;
                    existing.Quantity = newItem.Quantity;
                    existing.Price = finalPrice;
                    existing.DeletedAt = null;
                    existing.DeletedById = null;
                    unitOfWork.ItemOrders.Update(existing);
                }
                else
                {
                    var itemOrder = new ItemOrder
                    {
                        ItemId = item.Id,
                        OrderId = model.Id,
                        Quantity = newItem.Quantity,
                        Price = finalPrice
                    };
                    await unitOfWork.ItemOrders.AddAsync(itemOrder);
                }
                newItemsTotal += finalPrice * newItem.Quantity;
}
            order.TotalPrice = newItemsTotal;
            unitOfWork.Orders.Update(order);
            await unitOfWork.SaveChangesAsync();
            return true;
        }



        public async Task<POSOrdersViewModel> GetPOSOrdersAsync(
            POSOrdersFilterViewModel filter,
            CancellationToken cancellationToken = default)
        {
            // -----------------------------
            // Base query
            // Last 24 hours only
            // -----------------------------

            var fromDate = DateTime.UtcNow.AddHours(-24);

            var query = unitOfWork.Orders
                .NoTrackingSelect()
                .Where(o =>
                    o.OrderDate > fromDate &&
                    o.OrderStatus != OrderStatus.Cancelled &&
                    o.OrderStatus != OrderStatus.Completed);

            // -----------------------------
            // Search
            // TableId OR ItemName
            // -----------------------------

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var search = filter.Search.Trim();

                query = query.Where(o =>
                    EF.Functions.Like(
                        o.TableId.ToString(),
                        $"%{search}%")
                    ||
                    o.ItemOrders.Any(io =>
                        EF.Functions.Like(
                            io.Item.ItemName,
                            $"%{search}%"))
                );
            }

            // -----------------------------
            // Status filter
            // -----------------------------

            if (filter.Status.HasValue)
            {
                query = query.Where(o =>
                    o.OrderStatus == filter.Status.Value);
            }

            // -----------------------------
            // Sorting
            // -----------------------------

            query = filter.Sort switch
            {
                "newest" =>
                    query
                        .OrderByDescending(o => o.OrderDate)
                        .ThenByDescending(o => o.Id),

                "price-high" =>
                    query
                        .OrderByDescending(o => o.TotalPrice)
                        .ThenByDescending(o => o.OrderDate),

                "price-low" =>
                    query
                        .OrderBy(o => o.TotalPrice)
                        .ThenByDescending(o => o.OrderDate),

                _ =>
                    query
                        .OrderBy(o => o.OrderDate)
                        .ThenBy(o => o.Id)
            };

            // -----------------------------
            // Get ALL orders
            // -----------------------------

            var orders = await query
                .Select(o => new OrderViewModel
                {
                    Id = o.Id,
                    TableId = o.TableId,
                    OrderDate = o.OrderDate,
                    TotalPrice = o.TotalPrice,
                    OrderStatus = o.OrderStatus,

                    ItemOrders = o.ItemOrders
                        .Select(io => new ItemOrderViewModel
                        {
                            ItemId = io.ItemId,
                            OrderId = io.OrderId,
                            ItemName = io.Item.ItemName,
                            Quantity = io.Quantity,
                            Price = io.Price,

                            DiscountPercentage =
                                io.Item.Discount != null
                                    ? io.Item.Discount.DiscountPercentage
                                    : null
                        })
                        .ToList()
                })
                .ToListAsync(cancellationToken);

            return new POSOrdersViewModel
            {
                Orders = orders,
                Filter = filter,
                TotalCount = orders.Count
            };
        }
        public async Task<bool> UpdateOrderAsync(OrderStatusViewModel model)
        {
            if (model is null) throw new ArgumentNullException();
            var order = await unitOfWork.Orders.Select().Where(o => o.Id == model.OrderId).FirstOrDefaultAsync();
            if (order is null) return false;
            order.OrderStatus = model.Status;
            unitOfWork.Orders.Update(order);
            await unitOfWork.SaveChangesAsync(model.CancellationToken);
            return true;

        }
    }
}
