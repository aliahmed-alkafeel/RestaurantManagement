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
        public async Task<bool> DeleteOrderAsync(Guid modelId, Guid ModifierId)
        {
            var order = await unitOfWork.Orders.GetOrderWithItemsByIdAsync(modelId);
            if (order is null) throw new InvalidOperationException("There is no such order");
            unitOfWork.Orders.Delete(order, ModifierId);
            foreach(ItemOrder itemOrder in order.ItemOrders)
            {
            unitOfWork.ItemOrders.Delete(itemOrder, ModifierId);
            }
            await unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<List<OrderViewModel>> GetAllOrdersAsync()
        {
            var orders = await unitOfWork.Orders.GetAllOrdersWithItemsAsync();
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

        public async Task<OrderViewModel> GetOrderByIdAsync(Guid Id)
        {
            var order = await unitOfWork.Orders.GetOrderWithItemsByIdAsync(Id);
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
        public async Task<bool> UpdateOrderAsync(OrderViewModel model, Guid ModifierId)
        {
            if (model is null || model.ItemOrders is null) throw new ArgumentNullException();
            var order = await unitOfWork.Orders.GetOrderWithItemsByIdAsync(model.Id);
            if (order is null) return false;
            order.OrderStatus = model.OrderStatus;
            order.TableId = model.TableId;
            order.OrderDate = model.OrderDate;

            var existingItemOrders = order.ItemOrders.ToList();
            var ItemIds = model.ItemOrders.Select(x => x.ItemId).ToList();
            decimal newItemsTotal = 0;

            var toRemove = existingItemOrders.Where(io => !ItemIds.Contains(io.ItemId));
            
            foreach(var oldItemOrder in toRemove)
            {
                unitOfWork.ItemOrders.Delete(oldItemOrder,ModifierId);
            }
            
            foreach(var newItem in model.ItemOrders)
                {
                    var item = await unitOfWork.Items.GetByIdAsync(newItem.ItemId);
                    if(item is null) throw new ArgumentNullException("One of items is not exists");

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
                    unitOfWork.ItemOrders.Update(existing, ModifierId);
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
            unitOfWork.Orders.Update(order, ModifierId);
            await unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<List<OrderViewModel>> GetPOSOrders()
        {
            var orders = await unitOfWork.Orders.NoTrackingSelect().Where(o => o.OrderDate > DateTime.UtcNow.AddHours(-24)).OrderByDescending(o => o.OrderDate).ToListAsync();
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

        public async Task<bool> UpdateOrderAsync(OrderStatusViewModel model, Guid ModifierId)
        {
            if (model is null) throw new ArgumentNullException();
            var order = await unitOfWork.Orders.Select().Where(o => o.Id == model.OrderId).FirstOrDefaultAsync();
            if (order is null) return false;
            order.OrderStatus = model.Status;
            unitOfWork.Orders.Update(order, ModifierId);
            await unitOfWork.SaveChangesAsync();
            return true;

        }
    }
}
