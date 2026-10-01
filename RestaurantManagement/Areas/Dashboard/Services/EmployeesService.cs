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
                .Where(e => e.Group != null && !e.Group.IsDeleted)
                .Select(emp => new EmployeeViewModel
                {
                    Id = emp.Id,
                    FirstName = emp.FirstName,
                    LastName = emp.LastName,
                    PhoneNumber = emp.PhoneNumber,
                    EmployeeStartingDate = emp.EmployeeStartingDate.ToUtc(),
                    EmployeeEndingDate = emp.EmployeeEndingDate.HasValue?
                        emp.EmployeeEndingDate.Value.ToUtc() : null,
                    Username = emp.Username,
                    Email = emp.Email,
                    Group = emp.Group!.GroupName
                }).ToListAsync(cancellationToken);
        }

        public async Task<CreateEmployeeViewModel> GetCreateEmployeeByIdAsync(Guid id,
            CancellationToken cancellationToken = default)
        {
            var emp = await _unitOfWork.Employees.NoTrackingSelect()
                .Include(e => e.Group).Where(e=> e.Group != null && !e.Group.IsDeleted)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
            if (emp is null) throw new ArgumentNullException(nameof(emp));
            var groups = await _unitOfWork.Groups.NoTrackingSelect().Select(g =>
                new SelectListItem
                {
                    Value = g.GroupName,
                    Text = g.GroupName
                }).ToListAsync(cancellationToken);
            Console.WriteLine(emp.EmployeeStartingDate.ToUtc());
            return new CreateEmployeeViewModel
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

        public async Task<UpdateEmployeeViewModel> GetUpdateEmployeeByIdAsync(Guid id,
            CancellationToken cancellationToken = default)
        {
            var emp = await _unitOfWork.Employees.NoTrackingSelect()
                .Include(e => e.Group).Where(e=> e.Group != null && !e.Group.IsDeleted)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
            if (emp is null) throw new ArgumentNullException(nameof(emp));
            var groups = await _unitOfWork.Groups.NoTrackingSelect().Select(g =>
                new SelectListItem
                {
                    Value = g.GroupName,
                    Text = g.GroupName
                }).ToListAsync(cancellationToken);
            Console.WriteLine(emp.EmployeeStartingDate.ToUtc());
            return new UpdateEmployeeViewModel()
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
    
        public async Task<bool> CreateEmployeeAsync(CreateEmployeeViewModel model)
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
            var groupExists = await _unitOfWork.Groups
                .NoTrackingSelect()
                .AnyAsync(g => g.Id == model.Group, model.CancellationToken);

            if (!groupExists)
                return false;
            var modifierId = _httpContextAccessor.HttpContext!.User.GetUserId();
            var modifier = await _unitOfWork.Employees.NoTrackingSelect()
                .Include(e => e.Group)
                .Where(e => e.Group != null && !e.Group.IsDeleted)
                .FirstOrDefaultAsync(e => e.Id == modifierId, model.CancellationToken);
            if(modifier is null) throw new InvalidOperationException(nameof(modifier));
            if (modifier.Group is null) throw new InvalidOperationException("The Group Of the User is Deleted!");
            var administratorGroupName = InitUserGroup.Administrator.ToString();
            if (model.GroupName == administratorGroupName &&
                modifier!.Group!.GroupName != administratorGroupName)
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
            return await _unitOfWork.SaveChangesAsync(model.CancellationToken) > 0;

        }

        public async Task<bool> UpdateEmployeeAsync(UpdateEmployeeViewModel model)
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));
            var employee = await _unitOfWork.Employees
                .Select()
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
            var group = await _unitOfWork.Groups
                .NoTrackingSelect()
                .FirstOrDefaultAsync(
                    g => g.Id == model.Group,
                    model.CancellationToken);

            if (group is null)
                return false;

            var modifierId = _httpContextAccessor.HttpContext!
                .User
                .GetUserId();

            var modifier = await _unitOfWork.Employees
                .NoTrackingSelect()
                .Include(e => e.Group)
                .FirstOrDefaultAsync(
                    e => e.Id == modifierId &&
                         e.Group != null &&
                         !e.Group.IsDeleted,
                    model.CancellationToken);

            if (modifier is null)
                throw new InvalidOperationException(
                    "The current user does not have a valid group.");
            var administratorGroupName = InitUserGroup.Administrator.ToString();
            if (group.GroupName == administratorGroupName &&
                modifier.Group!.GroupName != administratorGroupName)
            {
                return false;
            }

            if (group.GroupName == administratorGroupName &&
                model.EmployeeEndingDate.HasValue)
            {
                return false;
            }
            employee.Username = model.Username;
            employee.Email = model.Email;
            employee.FirstName = model.FirstName;
            employee.LastName = model.LastName;
            employee.EmployeeStartingDate = model.EmployeeStartingDate;
            employee.EmployeeEndingDate = model.EmployeeEndingDate;
            employee.PhoneNumber = model.PhoneNumber;
            employee.GroupId = model.Group;

            if (model.Password is not null)
            {
                employee.PasswordHash = _passwordHasher.HashPassword(employee, model.Password);
            }
            _unitOfWork.Employees.Update(employee);
            return await _unitOfWork.SaveChangesAsync(model.CancellationToken) > 0;

        }

        public async Task<bool> TerminateEmployeeAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var emp = await _unitOfWork.Employees.Select()
                .Include(e => e.Group).Where(e => e.Group != null && !e.Group.IsDeleted)
                .FirstOrDefaultAsync(e => e.Id == id,cancellationToken);
            if (emp is null) return false;
            if (emp.Group!.GroupName == InitUserGroup.Administrator.ToString()) return false;
            if(emp.EmployeeEndingDate.HasValue) return false;
            emp.EmployeeEndingDate = DateTime.UtcNow;
            //_unitOfWork.Employees.Delete(emp);
            return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> DeleteEmployeeAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var emp = await _unitOfWork.Employees.Select()
                .Include(e => e.Group).Where(e => e.Group != null && !e.Group.IsDeleted)
                .FirstOrDefaultAsync(e => e.Id == id,cancellationToken);
            if (emp is null) return false;
            if (emp.Group!.GroupName == InitUserGroup.Administrator.ToString()) return false;
            //if(emp.EmployeeEndingDate.HasValue) return false;
            emp.EmployeeEndingDate ??= DateTime.UtcNow;
            _unitOfWork.Employees.Delete(emp);
            return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}