using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.IRepositories;
using RestaurantManagement.IServices;
using RestaurantManagement.Models;
using RestaurantManagement.ViewModels;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace RestaurantManagement.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<Employee> _passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthService(IUnitOfWork unitOfWork, IPasswordHasher<Employee> passwordHasher,IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<bool> LoginAsync(LoginViewModel loginViewModel)
        {
            var employee = await _unitOfWork.Employees.NoTrackingSelect()
                .Include(e => e.Group)
                .FirstOrDefaultAsync(e => e.Username == loginViewModel.Username, loginViewModel.CancellationToken);
            if (employee is null) return false;
            if (employee.EmployeeEndingDate.HasValue) return false;
            var result = _passwordHasher.VerifyHashedPassword(employee, employee.PasswordHash, loginViewModel.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return false;
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
                new Claim(ClaimTypes.Name, employee.Username),
            };
            var group = await _unitOfWork.Groups.NoTrackingSelect().Include(g => g.GroupRoles).ThenInclude(gr => gr.Role)
                .FirstOrDefaultAsync(g => g.Id == employee.GroupId, loginViewModel.CancellationToken);
            if (group is null) throw new InvalidOperationException("The Group Of the User is Deleted!");
            claims.AddRange(group.GroupRoles.Select(role => new Claim(ClaimTypes.Role, role.Role.RoleName.ToString())));
            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var properties = new AuthenticationProperties { IsPersistent = loginViewModel.RememberMe };
            await _httpContextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal,properties);
            return true;
        }

        public async Task LogoutAsync()
        {
            await _httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
