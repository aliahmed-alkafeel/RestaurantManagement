using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.Models;

namespace RestaurantManagement.Areas.Dashboard.IServices
{
    public interface ICategoriesService
    {
        Task<List<CategoryViewModel>> GetAllCategoriesAsync(CancellationToken cancellationToken);
        Task<CategoryViewModel> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> CreateCategoryAsync(CategoryViewModel model, CancellationToken cancellationToken = default);
        Task<bool> UpdateCategoryAsync(CategoryViewModel model);
        Task<bool> DeleteCategoryAsync(Guid modelId);
    }
}
