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
        public async Task<bool> DeleteOrderAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var order = await unitOfWork.Orders.Select()
                .Include(o => o.ItemOrders).ThenInclude(io => io.Item)
                .ThenInclude(i => i.Discount).FirstOrDefaultAsync(o => o.Id == id,cancellationToken);
            if (order is null) throw new InvalidOperationException("There is no such order");
            unitOfWork.Orders.Delete(order); 
            unitOfWork.ItemOrders.DeleteRange(order.ItemOrders);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<List<OrderViewModel>> GetAllOrdersAsync(CancellationToken cancellationToken = default)
        {
            return await unitOfWork.Orders.NoTrackingSelect().OrderByDescending(o => o.OrderDate)
                .Include(o => o.ItemOrders)
                .ThenInclude(io => io.Item).ThenInclude(i => i.Discount)
               .Select(order => new OrderViewModel
                {
                    Id = order.Id,
                    TableId = order.TableId,
                    OrderDate = order.OrderDate,
                    OrderStatus = order.OrderStatus,
                    TotalPrice = order.TotalPrice
                }).ToListAsync(cancellationToken);
        }

        public async Task<OrderViewModel> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var order = await unitOfWork.Orders.NoTrackingSelect()
                .Include(o => o.ItemOrders).ThenInclude(io => io.Item)
                .ThenInclude(i => i.Discount).FirstOrDefaultAsync(o => o.Id == id,cancellationToken);
            if (order is null) throw new ArgumentNullException(nameof(order));
            return new OrderViewModel()
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
                                         x.Item.Discount.DiscountEndingDate >= DateTime.UtcNow
                        ? x.Item.Price * (1 - (x.Item.Discount.DiscountPercentage / 100))
                        : x.Item.Price
                }).ToList()
            };
        
        }

        public async Task<bool> CreateOrderAsync(CreateOrderViewModel model)
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));

            var newItems = model.ItemOrders
                .Where(x => x.Quantity > 0)
                .GroupBy(x => x.ItemId)
                .Select(g => new
                {
                    ItemId = g.Key,
                    Quantity = g.Sum(x => x.Quantity)
                })
                .ToList();

            if (newItems.Count == 0)
                return false;

            var now = DateTime.UtcNow;

            var itemIds = newItems
                .Select(x => x.ItemId)
                .ToList();

            var items = await unitOfWork.Items
                .NoTrackingSelect().Where(i => itemIds.Contains(i.Id))
                .Include(i => i.Discount)
                .ToListAsync(model.CancellationToken);

            var itemsById = items.ToDictionary(x => x.Id);

            var order = new Order
            {
                Id = Guid.NewGuid(),
                OrderStatus = OrderStatus.Pending,
                TableId = model.TableId,
                OrderDate = now
            };

            decimal totalPrice = 0;

            foreach (var newItem in newItems)
            {
                if (!itemsById.TryGetValue(newItem.ItemId, out var item))
                {
                    throw new InvalidOperationException(
                        $"Item with ID '{newItem.ItemId}' does not exist.");
                }

                if (!item.IsAvailable || !item.IsActive)
                {
                    throw new InvalidOperationException(
                        $"Item '{item.ItemName}' is not available.");
                }

                var discount = item.Discount;

                var finalPrice = discount is not null &&
                                 discount.DiscountStartingDate <= now &&
                                 discount.DiscountEndingDate >= now
                    ? item.Price * (1 - discount.DiscountPercentage / 100)
                    : item.Price;

                var itemOrder = new ItemOrder
                {
                    ItemId = item.Id,
                    OrderId = order.Id,
                    Quantity = (short) newItem.Quantity,
                    Price = finalPrice
                };

                await unitOfWork.ItemOrders.AddAsync(
                    itemOrder,
                    model.CancellationToken);

                totalPrice += finalPrice * newItem.Quantity;
            }

            order.TotalPrice = totalPrice;

            await unitOfWork.Orders.AddAsync(
                order,
                model.CancellationToken);

            await unitOfWork.SaveChangesAsync(model.CancellationToken);

            return true;
        }
        public async Task<bool> UpdateOrderAsync(OrderViewModel model)
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));

            if (model.ItemOrders is null)
                throw new ArgumentNullException(nameof(model.ItemOrders));

            var order = await unitOfWork.Orders
                .Select()
                .Include(o => o.ItemOrders)
                .ThenInclude(io => io.Item)
                .ThenInclude(i => i.Discount)
                .FirstOrDefaultAsync(
                    o => o.Id == model.Id,
                    model.CancellationToken);

            if (order is null)
                return false;

            var now = DateTime.UtcNow;

            var newItems = model.ItemOrders
                .Where(x => x.Quantity > 0)
                .GroupBy(x => x.ItemId)
                .Select(g => new
                {
                    ItemId = g.Key,
                    Quantity = (short) g.Sum(x => x.Quantity)
                })
                .ToList();

            if (newItems.Count == 0)
                return false;

            var newItemIds = newItems
                .Select(x => x.ItemId)
                .ToHashSet();

            var existingItemOrders = order.ItemOrders.ToList();

            foreach (var itemOrder in existingItemOrders)
            {
                if (!newItemIds.Contains(itemOrder.ItemId))
                {
                    unitOfWork.ItemOrders.Delete(itemOrder);
                }
            }

            var items = await unitOfWork.Items
                .NoTrackingSelect()
                .Include(i => i.Discount)
                .Where(i => newItemIds.Contains(i.Id))
                .ToListAsync(model.CancellationToken);

            var itemsById = items.ToDictionary(i => i.Id);

            var existingByItemId = existingItemOrders
                .ToDictionary(io => io.ItemId);

            decimal newItemsTotal = 0;

            foreach (var newItem in newItems)
            {
                if (!itemsById.TryGetValue(newItem.ItemId, out var item))
                {
                    throw new InvalidOperationException(
                        $"Item with ID '{newItem.ItemId}' does not exist.");
                }

                //if (!item.IsAvailable || !item.IsActive)
                //{
                //    throw new InvalidOperationException(
                //        $"Item '{item.ItemName}' is not available.");
                //}

                var discount = item.Discount;

                var finalPrice =
                    discount is not null &&
                    discount.DiscountStartingDate <= now &&
                    discount.DiscountEndingDate >= now
                        ? item.Price * (1 - discount.DiscountPercentage / 100)
                        : item.Price;

                if (existingByItemId.TryGetValue(newItem.ItemId, out var existingItemOrder))
                {
                    if (existingItemOrder.Quantity == newItem.Quantity &&
                        existingItemOrder.Price == finalPrice)
                    {
                        newItemsTotal += finalPrice * newItem.Quantity;
                        continue;
                    }

                    unitOfWork.ItemOrders.Delete(existingItemOrder);

                    var newItemOrder = new ItemOrder
                    {
                        ItemId = item.Id,
                        OrderId = order.Id,
                        Quantity =  newItem.Quantity,
                        Price = finalPrice
                    };

                    await unitOfWork.ItemOrders.AddAsync(
                        newItemOrder,
                        model.CancellationToken);
                }
                else
                {
                    var newItemOrder = new ItemOrder
                    {
                        ItemId = item.Id,
                        OrderId = order.Id,
                        Quantity = newItem.Quantity,
                        Price = finalPrice
                    };

                    await unitOfWork.ItemOrders.AddAsync(
                        newItemOrder,
                        model.CancellationToken);
                }

                newItemsTotal += finalPrice * newItem.Quantity;
            }

            order.OrderStatus = model.OrderStatus;
            order.TableId = model.TableId;
            order.OrderDate = model.OrderDate.AddHours(-3);
            order.TotalPrice = newItemsTotal;
            unitOfWork.Orders.Update(order);
            await unitOfWork.SaveChangesAsync(model.CancellationToken);

            return true;
        }

        public async Task<POSOrdersViewModel> GetPOSOrdersAsync(
            POSOrdersFilterViewModel filter)
        {

            var fromDate = DateTime.UtcNow.AddHours(-24);

            var query = unitOfWork.Orders
                .NoTrackingSelect()
                .Where(o =>
                    o.OrderDate > fromDate &&
                    o.OrderStatus != OrderStatus.Cancelled &&
                    o.OrderStatus != OrderStatus.Completed);


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


            if (filter.Status.HasValue)
            {
                query = query.Where(o =>
                    o.OrderStatus == filter.Status.Value);
            }


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
                .ToListAsync(filter.CancellationToken);

            return new POSOrdersViewModel
            {
                Orders = orders,
                Filter = filter,
                TotalCount = orders.Count
            };
        }

        public async Task<bool> UpdateOrderAsync(OrderStatusViewModel model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));
            var order = await unitOfWork.Orders.Select()
                .FirstOrDefaultAsync(o => o.Id == model.OrderId,model.CancellationToken);
            if (order is null) return false;
            order.OrderStatus = model.Status;
            unitOfWork.Orders.Update(order);
            await unitOfWork.SaveChangesAsync(model.CancellationToken);
            return true;
        }
    }
}
