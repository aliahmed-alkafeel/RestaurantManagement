using RestaurantManagement.Areas.Dashboard.ViewModels;

namespace RestaurantManagement.Areas.Dashboard.IServices
{
    public interface IDiscountService
    {
        Task<List<DiscountViewModel>> GetAllDiscountsAsync(CancellationToken cancellationToken = default);
        Task<DiscountViewModel> GetDiscountByIdAsync(Guid id,CancellationToken cancellationToken = default);
        Task<bool> CreateDiscountAsync(DiscountViewModel model);
        Task<bool> UpdateDiscountAsync(DiscountViewModel model);
        Task<bool> DeleteDiscountAsync(Guid modelId,CancellationToken cancellationToken = default);
    }
}
