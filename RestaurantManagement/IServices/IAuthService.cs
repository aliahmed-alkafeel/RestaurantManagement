using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.ViewModels;

namespace RestaurantManagement.IServices
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
    }
}
