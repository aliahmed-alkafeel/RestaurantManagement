using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using RestaurantManagement.IRepositories;
using RestaurantManagement.IServices;
using RestaurantManagement.Models;
using RestaurantManagement.ViewModels;
using System.Security.Claims;

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
            var employee = await _unitOfWork.Employees.GetEmployeeWithGroupByUsernameAsync(loginViewModel.Username);
            if (employee is null)
            {
                return false;
            }
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
            var group = await _unitOfWork.Groups.GetGroupWithRolesByIdAsync(employee.GroupId);
            foreach(var role in group.GroupRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Role.RoleName.ToString()));
            }
            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var properties = new AuthenticationProperties { IsPersistent = loginViewModel.RememberMe };
            await _httpContextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal,properties);
            return true;
        }
    }
}
