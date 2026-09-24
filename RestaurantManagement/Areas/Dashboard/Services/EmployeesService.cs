using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Areas.Dashboard.IServices;
using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.IRepositories;
using RestaurantManagement.Models;
using RestaurantManagement.Repositories;
using System.Security.Claims;
using RestaurantManagement.Extensions;

namespace RestaurantManagement.Areas.Dashboard.Services
{
    public class EmployeesService : IEmployeesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<Employee> _passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmployeesService(IUnitOfWork unitOfWork, IPasswordHasher<Employee> passwordHasher,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<EmployeeViewModel>> GetAllEmployeesAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Employees.NoTrackingSelect()
                .Include(e => e.Group)
                .Select(emp => new EmployeeViewModel
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
                }).ToListAsync(cancellationToken);
        }

        public async Task<ManageEmployeeViewModel> GetEmployeeByIdAsync(Guid id,
            CancellationToken cancellationToken = default)
        {
            var emp = await _unitOfWork.Employees.NoTrackingSelect()
                .Include(e => e.Group)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
            if (emp is null) throw new ArgumentNullException(nameof(emp));
            var groups = await _unitOfWork.Groups.NoTrackingSelect().Select(g =>
                new SelectListItem
                {
                    Value = g.GroupName,
                    Text = g.GroupName
                }).ToListAsync(cancellationToken);
            return new ManageEmployeeViewModel
            {
                Id = emp.Id,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                PhoneNumber = emp.PhoneNumber,
                EmployeeStartingDate = emp.EmployeeStartingDate,
                EmployeeEndingDate = emp.EmployeeEndingDate,
                Username = emp.Username,
                Email = emp.Email,
                Group = emp.GroupId,
                GroupName = emp.Group!.GroupName,
                Groups = groups
            };
        }

        public async Task<List<SelectListItem>> ShowCreateEmployeeAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Groups.NoTrackingSelect().Select(g =>
                new SelectListItem
                {
                    Value = g.Id.ToString(),
                    Text = g.GroupName
                }).ToListAsync(cancellationToken);
        }
    
        public async Task<bool> CreateEmployeeAsync(ManageEmployeeViewModel model)
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));
            var isDuplicate = await _unitOfWork.Employees
                .NoTrackingSelect()
                .AnyAsync(
                    e => e.Id != model.Id &&
                         (!e.EmployeeEndingDate.HasValue && 
                          (e.Email == model.Email || e.Username == model.Username)),
                    model.CancellationToken);
            if (isDuplicate)
                return false;
            var modifierId = _httpContextAccessor.HttpContext!.User.GetUserId();
            var modifier = await _unitOfWork.Employees.NoTrackingSelect(DeletedStatus.All)
                .Include(e => e.Group)
                .FirstOrDefaultAsync(e => e.Id == modifierId, model.CancellationToken);
            if(modifier is null) throw new InvalidOperationException(nameof(modifier));
            if (modifier.Group is null) throw new InvalidOperationException("The Group Of the User is Deleted!");
            if (model.GroupName == InitUserGroup.Administrator.ToString() &&
                modifier!.Group!.GroupName != InitUserGroup.Administrator.ToString())
                return false;
            var employee = new Employee
            {
                Username = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmployeeStartingDate = DateTime.UtcNow,
                PhoneNumber = model.PhoneNumber,
                GroupId = model.Group
            };
            employee.PasswordHash = _passwordHasher.HashPassword(employee, model.Password);
            await _unitOfWork.Employees.AddAsync(employee, model.CancellationToken);
            await _unitOfWork.SaveChangesAsync(model.CancellationToken);
            return true;
        }

        public async Task<bool> UpdateEmployeeAsync(ManageEmployeeViewModel model)
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));
            var employee = await _unitOfWork.Employees
                .Select(DeletedStatus.All)
                .FirstOrDefaultAsync(
                    e => e.Id == model.Id,
                    model.CancellationToken);

            if (employee is null)
                return false;

            var isDuplicate = await _unitOfWork.Employees
                .NoTrackingSelect()
                .AnyAsync(
                    e => e.Id != model.Id &&
                         (e.Email == model.Email || e.Username == model.Username),
                    model.CancellationToken);

            if (isDuplicate)
                return false;

            employee.Username = model.Username;
            employee.Email = model.Email;
            employee.FirstName = model.FirstName;
            employee.LastName = model.LastName;
            employee.EmployeeStartingDate = model.EmployeeStartingDate;
            employee.PhoneNumber = model.PhoneNumber;
            employee.GroupId = model.Group;
            employee.EmployeeEndingDate = model.EmployeeEndingDate;

            if (model.Password is not null)
            {
                employee.PasswordHash = _passwordHasher.HashPassword(employee, model.Password);
            }
            _unitOfWork.Employees.Update(employee);
            await _unitOfWork.SaveChangesAsync(model.CancellationToken);
            return true;
        }

        public async Task<bool> TerminateEmployeeAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var emp = await _unitOfWork.Employees.Select(DeletedStatus.All)
                .Include(e => e.Group).Where(e => e.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
            if (emp is null) return false;
            if (emp.Group!.GroupName == InitUserGroup.Administrator.ToString()) return false;
            if(emp.EmployeeEndingDate.HasValue) return false;
            emp.EmployeeEndingDate = DateTime.UtcNow;
            //_unitOfWork.Employees.Delete(emp);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> DeleteEmployeeAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var emp = await _unitOfWork.Employees.Select(DeletedStatus.All)
                .Include(e => e.Group).Where(e => e.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
            if (emp is null) return false;
            if (emp.Group!.GroupName == InitUserGroup.Administrator.ToString()) return false;
            //if(emp.EmployeeEndingDate.HasValue) return false;
            emp.EmployeeEndingDate ??= DateTime.UtcNow;
            _unitOfWork.Employees.Delete(emp);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}