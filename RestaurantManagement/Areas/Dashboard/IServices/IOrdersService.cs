using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.Models;
using RestaurantManagement.ViewModels;

namespace RestaurantManagement.Areas.Dashboard.IServices
{
    public interface IOrdersService
    {
        Task<List<OrderViewModel>> GetAllOrdersAsync();
        Task<OrderViewModel> GetOrderByIdAsync(Guid Id);
        Task<bool> UpdateOrderAsync(OrderViewModel model, Guid ModifierId);
        Task<bool> CreateOrderAsync(CreateOrderViewModel model);
        Task<bool> DeleteOrderAsync(Guid id, Guid ModifierId);
        Task<List<OrderViewModel>> GetPOSOrders();
    }
}
