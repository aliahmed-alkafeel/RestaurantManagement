using Microsoft.AspNetCore.Mvc.Rendering;
using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.Models;

namespace RestaurantManagement.Areas.Dashboard.IServices
{
    public interface IEmployeesService
    {
        Task<List<EmployeeViewModel>> GetAllEmployeesAsync(CancellationToken cancellationToken = default);
        Task<CreateEmployeeViewModel> GetCreateEmployeeByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<UpdateEmployeeViewModel> GetUpdateEmployeeByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> CreateEmployeeAsync(CreateEmployeeViewModel model);
        Task<bool> UpdateEmployeeAsync(UpdateEmployeeViewModel model);
        Task<bool> TerminateEmployeeAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> DeleteEmployeeAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<SelectListItem>> ShowCreateEmployeeAsync(CancellationToken cancellationToken = default);
    }
}
