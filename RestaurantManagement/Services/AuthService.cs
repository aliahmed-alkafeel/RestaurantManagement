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
            var employee = await _unitOfWork.Employees.GetEmployeeByUsernameAsync(loginViewModel.Username);
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
            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await _httpContextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal);
            return true;
        }
    }
}
