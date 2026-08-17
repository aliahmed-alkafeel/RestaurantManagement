using RestaurantManagement.Models;
using RestaurantManagement.ViewModels;

namespace RestaurantManagement.IServices
{
    public interface IDashboardService
    {
        Task<List<EmployeeViewModel>> GetAllEmployeesAsync();
        Task UpdateEmployeeAsync(Employee emp);
        Task DeleteEmployeeAsync(Employee emp);
    }
}
