using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.Models;
using RestaurantManagement.ViewModels;

namespace RestaurantManagement.Areas.Dashboard.IServices
{
    public interface IOrdersService
    {
        Task<List<OrderViewModel>> GetAllOrdersAsync(CancellationToken cancellationToken = default);
        Task<OrderViewModel> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> UpdateOrderAsync(OrderViewModel model);
        Task<bool> UpdateOrderAsync(OrderStatusViewModel model);
        Task<bool> CreateOrderAsync(CreateOrderViewModel model);
        Task<bool> DeleteOrderAsync(Guid id, CancellationToken cancellationToken = default);
        Task<POSOrdersViewModel> GetPOSOrdersAsync(
            POSOrdersFilterViewModel filter);
    }
}
