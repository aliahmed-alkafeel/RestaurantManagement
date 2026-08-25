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
            var order = await unitOfWork.Orders.GetByIdAsync(modelId);
            if (order is null) throw new InvalidOperationException("There is no such order");
            unitOfWork.Orders.Delete(order, ModifierId);
            await unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<List<OrderViewModel>> GetAllOrdersAsync()
        {
            var orders = await unitOfWork.Orders.GetAllOrdersWithItemsAsync();
            List<OrderViewModel> ordersVm = [];
            foreach (Order order in orders)
            {
                decimal totalPrice = 0;
                foreach(ItemOrder itemOrder in order.ItemOrders)
                {
                    decimal totalWithOutDiscount = itemOrder.Price * itemOrder.Quantity;
                    var discount = itemOrder.Item;
                }
                {
                    ordersVm.Add(new OrderViewModel
                    {
                        Id = order.Id,
                        OrderDate = DateTime.UtcNow,
                        OrderStatus = order.OrderStatus,
                     });
                }
            }
            return ordersVm;

        }

        public async Task<OrderViewModel> GetOrderByIdAsync(Guid Id)
        {
            var order = await unitOfWork.Orders.GetByIdAsync(Id);
            if (order is null) throw new KeyNotFoundException("There is no such order");
            OrderViewModel orderVm = new OrderViewModel
            {
                Id = order.Id,
                OrderName = order.OrderName,
                Type = order.Type
            };
            return orderVm;
        }

        public async Task<List<OrderViewModel>> OrderDetailsAsync(OrderViewModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateOrderAsync(OrderViewModel model, Guid ModifierId)
        {
            var orders = await unitOfWork.Orders.GetAllAsync();
            foreach (Order cat in orders)
            {
                if ((cat.OrderName == model.OrderName && cat.Id != model.Id) &&
                    (cat.Type == model.Type && cat.Id != model.Id))
                {
                    return false;
                }
            }
            var order = orders.FirstOrDefault(c => c.Id == model.Id);
            if (order is null) return false;
            order.OrderName = model.OrderName;
            order.Type = model.Type;
            unitOfWork.Orders.Update(order, ModifierId);
            await unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
