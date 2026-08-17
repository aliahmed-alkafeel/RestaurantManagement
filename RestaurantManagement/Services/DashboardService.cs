using RestaurantManagement.IRepositories;
using RestaurantManagement.IServices;
using RestaurantManagement.Models;
using RestaurantManagement.Repositories;
using RestaurantManagement.ViewModels;

namespace RestaurantManagement.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task DeleteEmployeeAsync(Employee emp)
        {
            //_unitOfWork.Employees.Delete(emp,)
            throw new NotImplementedException();
        }

        public async Task<List<EmployeeViewModel>> GetAllEmployeesAsync()
        {
            var emps = await _unitOfWork.Employees.GetAllEmployeesWithGroupsAsync();
            List<EmployeeViewModel> empvm = [];
            foreach(Employee emp in emps)
            {
                empvm.Add(new EmployeeViewModel
                {
                    FirstName = emp.FirstName,
                    PhoneNumber = emp.PhoneNumber,
                    EmployeeStartingDate = emp.EmployeeStartingDate,
                    EmployeeEndingDate = emp.EmployeeEndingDate,
                    Username = emp.Username,
                    Email = emp.Email,
                    Group = emp.Group!.GroupName
                });
            }
            return empvm; 
        }

        public Task UpdateEmployeeAsync(Employee emp)
        {
            throw new NotImplementedException();
        }
    }
}
