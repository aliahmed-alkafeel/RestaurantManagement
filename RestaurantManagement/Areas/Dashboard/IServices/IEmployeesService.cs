using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.Models;

namespace RestaurantManagement.Areas.Dashboard.IServices
{
    public interface IEmployeesService
    {
        Task<List<EmployeeViewModel>> GetAllEmployeesAsync();
        Task<ManageEmployeeViewModel> GetEmployeeByIdAsync(Guid Id);
        Task<bool> CreateEmployeeAsync(ManageEmployeeViewModel model);
        Task<bool> UpdateEmployeeAsync(ManageEmployeeViewModel model, Guid ModifierId);
        Task<bool> TerminateEmployeeAsync(Guid modelId, Guid ModifierId);
    }
}
