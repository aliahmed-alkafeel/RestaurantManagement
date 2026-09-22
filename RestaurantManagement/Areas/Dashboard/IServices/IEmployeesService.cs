using Microsoft.AspNetCore.Mvc.Rendering;
using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.Models;

namespace RestaurantManagement.Areas.Dashboard.IServices
{
    public interface IEmployeesService
    {
        Task<List<EmployeeViewModel>> GetAllEmployeesAsync(CancellationToken cancellationToken = default);
        Task<ManageEmployeeViewModel> GetEmployeeByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> CreateEmployeeAsync(ManageEmployeeViewModel model);
        Task<bool> UpdateEmployeeAsync(ManageEmployeeViewModel model);
        Task<bool> TerminateEmployeeAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<SelectListItem>> ShowCreateEmployeeAsync(CancellationToken cancellationToken = default);
    }
}
