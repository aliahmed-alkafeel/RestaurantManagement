using RestaurantManagement.ViewModels;

namespace RestaurantManagement.IServices
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(RegisterViewModel model);
    }
}
