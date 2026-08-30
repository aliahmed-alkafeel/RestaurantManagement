using RestaurantManagement.Areas.Dashboard.IServices;
using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.IRepositories;
using RestaurantManagement.Models;
using RestaurantManagement.Repositories;

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
                    Quantity = x.Quantity
                }).ToList()
            };
            return orderVm;
        }

        public async Task<bool> UpdateOrderAsync(OrderViewModel model, Guid ModifierId)
        {
            if (model is null || model.ItemOrders is null) throw new ArgumentNullException();
            var order = await unitOfWork.Orders.GetOrderWithItemsByIdAsync(model.Id);
            if (order is null) return false;
            order.OrderStatus = model.OrderStatus;
            order.TableId = model.TableId;
            order.TotalPrice = model.TotalPrice;
            order.OrderDate = model.OrderDate;

            var existingItemOrders = order.ItemOrders.ToList();
            var ItemIds = model.ItemOrders.Select(x => x.ItemId).ToList();
            decimal newItemsTotal = 0;

            var toRemove = existingItemOrders.Where(io => !io.IsDeleted && !ItemIds.Contains(io.ItemId));
            
            foreach(var oldItemOrder in toRemove)
            {
                unitOfWork.ItemOrders.Delete(oldItemOrder,ModifierId);
            }
            
            foreach(var newItem in model.ItemOrders)
                {
                    var item = await unitOfWork.Items.GetByIdAsync(newItem.ItemId);
                    if(item is null) throw new ArgumentNullException("One of items is not exists");
                    var existing = existingItemOrders.FirstOrDefault(io => io.ItemId == newItem.ItemId);
                if (existing is not null)
                {
                    existing.IsDeleted = false;
                    existing.Quantity = newItem.Quantity;
                    existing.Price = item.Price;
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
                        Price = item.Price,
                    };
                    await unitOfWork.ItemOrders.AddAsync(itemOrder);
                }
                newItemsTotal += item.Price * newItem.Quantity;
            }
            order.TotalPrice = newItemsTotal;
            unitOfWork.Orders.Update(order, ModifierId);
            await unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
