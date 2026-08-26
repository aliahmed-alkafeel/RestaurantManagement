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
                ItemOrders = order.ItemOrders
            };
            return orderVm;
        }

        public async Task<bool> UpdateOrderAsync(OrderViewModel model, Guid ModifierId)
        {
            if (model is null) throw new ArgumentNullException();
            var order = await unitOfWork.Orders.GetOrderWithItemsByIdAsync(model.Id);
            if (order is null) return false;
            order.OrderStatus = model.OrderStatus;
            order.TableId = model.TableId;
            order.TotalPrice = model.TotalPrice;
            order.OrderDate = model.OrderDate;
            order.ItemOrders = model.ItemOrders;
            unitOfWork.Orders.Update(order, ModifierId);
            await unitOfWork.SaveChangesAsync();
            return true;
        }

    }
}
