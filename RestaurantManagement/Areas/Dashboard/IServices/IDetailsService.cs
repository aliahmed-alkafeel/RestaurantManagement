using RestaurantManagement.Areas.Dashboard.ViewModels;

namespace RestaurantManagement.Areas.Dashboard.IServices
{
    public interface IDetailsService
    {
        Task<DetailsViewModel> GetDetailsAsync();
    }
}