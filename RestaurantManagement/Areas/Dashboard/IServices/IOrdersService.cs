using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.Models;
using RestaurantManagement.ViewModels;

namespace RestaurantManagement.Areas.Dashboard.IServices
{
    public interface IOrdersService
    {
        Task<List<OrderViewModel>> GetAllOrdersAsync();
        Task<OrderViewModel> GetOrderByIdAsync(Guid Id);
        Task<bool> UpdateOrderAsync(OrderViewModel model);
        Task<bool> UpdateOrderAsync(OrderStatusViewModel model);
        Task<bool> CreateOrderAsync(CreateOrderViewModel model);
        Task<bool> DeleteOrderAsync(Guid id);
        Task<POSOrdersViewModel> GetPOSOrdersAsync(
            POSOrdersFilterViewModel filter,
            CancellationToken cancellationToken = default);
    }
}
