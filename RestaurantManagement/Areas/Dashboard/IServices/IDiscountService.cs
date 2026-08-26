using RestaurantManagement.Areas.Dashboard.ViewModels;

namespace RestaurantManagement.Areas.Dashboard.IServices
{
    public interface IDiscountService
    {
        Task<List<DiscountViewModel>> GetAllDiscountsAsync();
        Task<DiscountViewModel> GetDiscountByIdAsync(Guid Id);
        Task<bool> CreateDiscountAsync(DiscountViewModel model);
        Task<bool> UpdateDiscountAsync(DiscountViewModel model, Guid ModifierId);
        Task<bool> DeleteDiscountAsync(Guid modelId, Guid ModifierId);
    }
}
