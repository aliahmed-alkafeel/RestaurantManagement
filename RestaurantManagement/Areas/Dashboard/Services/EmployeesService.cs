using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Areas.Dashboard.IServices;
using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.IRepositories;
using RestaurantManagement.Models;
using RestaurantManagement.Repositories;

namespace RestaurantManagement.Areas.Dashboard.Services
{
    public class EmployeesService : IEmployeesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<Employee> _passwordHasher;
        public EmployeesService(IUnitOfWork unitOfWork, IPasswordHasher<Employee> passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task<List<EmployeeViewModel>> GetAllEmployeesAsync()
        {
            var emps = await _unitOfWork.Employees.GetAllEmployeesWithGroupsAsync();
            List<EmployeeViewModel> empvm = [];
            foreach (Employee emp in emps)
            {
                empvm.Add(new EmployeeViewModel
                {
                    Id = emp.Id,
                    FirstName = emp.FirstName,
                    LastName = emp.LastName,
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

        public async Task<ManageEmployeeViewModel> GetEmployeeByIdAsync(Guid id)
        {
            var emp = await _unitOfWork.Employees.GetEmployeeWithGroupAsync(id);
            if (emp is null) throw new EntryPointNotFoundException("There is no such employee");
            var groups = await _unitOfWork.Groups.NoTrackingSelect().Select(g =>
            new SelectListItem{
                Value = g.Id.ToString(),
                Text = g.GroupName }).ToListAsync();
            ManageEmployeeViewModel empvm = new ManageEmployeeViewModel
            {
                Id = emp.Id,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                PhoneNumber = emp.PhoneNumber,
                EmployeeStartingDate = emp.EmployeeStartingDate,
                EmployeeEndingDate = emp.EmployeeEndingDate,
                Username = emp.Username,
                Email = emp.Email,
                Group = emp.Group!.GroupName,
                Groups = groups
            };
            return empvm;
        }


        public async Task<bool> CreateEmployeeAsync(ManageEmployeeViewModel model)
        {
            if (model is null) throw new ArgumentNullException();
            var existingEmployee = await _unitOfWork.Employees.GetEmployeeByUsernameAsync(model.Username);
            if (existingEmployee is not null)
            {
           
                return false;
            }
            var groupId = await _unitOfWork.Groups.GetIdByNameAsync(model.Group);
            var employee = new Employee
            {
                Username = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmployeeStartingDate = DateTime.UtcNow,
                PhoneNumber = model.PhoneNumber,
                GroupId = groupId
            };
            employee.PasswordHash = _passwordHasher.HashPassword(employee, model.Password);
            await _unitOfWork.Employees.AddAsync(employee);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateEmployeeAsync(ManageEmployeeViewModel model, Guid ModifierId)
        {
            if (model is null) throw new ArgumentNullException();
            var isEmailExists = await _unitOfWork.Employees.GetEmployeeByEmailAsync(model.Email);
        var isUsernameExists = await _unitOfWork.Employees.GetEmployeeByUsernameAsync(model.Username);
        if((isEmailExists is not null && isEmailExists.Id != model.Id) || (isUsernameExists is not null && isUsernameExists.Id != model.Id)){
                return false;
            }
        var employee = await _unitOfWork.Employees.GetByIdAsync(model.Id);
            if (employee is null || (model.EmployeeEndingDate.HasValue && model.EmployeeEndingDate <= model.EmployeeStartingDate))
                return false;

            var groupId = await _unitOfWork.Groups.GetIdByNameAsync(model.Group);

            employee.Username = model.Username;
            employee.Email = model.Email;
            employee.FirstName = model.FirstName;
            employee.LastName = model.LastName;
            employee.EmployeeStartingDate = model.EmployeeStartingDate;
            employee.PhoneNumber = model.PhoneNumber;
            employee.GroupId = groupId;
            employee.EmployeeEndingDate = model.EmployeeEndingDate;

        if(model.Password is not null && model.Password == model.ConfirmPassword)
        employee.PasswordHash = _passwordHasher.HashPassword(employee, model.Password);
        _unitOfWork.Employees.Update(employee,ModifierId);
        await _unitOfWork.SaveChangesAsync();
            return true;

        }

        public async Task<bool> TerminateEmployeeAsync(Guid modelId, Guid ModifierId)
        {
            var emp = await _unitOfWork.Employees.GetByIdAsync(modelId);
            if (emp is null) throw new InvalidOperationException("There is no such employee");
            _unitOfWork.Employees.Terminate(emp,ModifierId);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }


    }
}
