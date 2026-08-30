using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.Models;

namespace RestaurantManagement.Areas.Dashboard.IServices
{
    public interface ICategoriesService
    {
        Task<List<CategoryViewModel>> GetAllCategoriesAsync();
        Task<CategoryViewModel> GetCategoryByIdAsync(Guid Id);
        Task<bool> CreateCategoryAsync(CategoryViewModel model);
        Task<bool> UpdateCategoryAsync(CategoryViewModel model, Guid ModifierId);
        Task<bool> DeleteCategoryAsync(Guid modelId, Guid ModifierId);
    }
}
