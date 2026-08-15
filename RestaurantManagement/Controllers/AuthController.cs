using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.IServices;
using RestaurantManagement.Models;
using RestaurantManagement.ViewModels;

namespace RestaurantManagement.Controllers
{
    public class AuthController : Controller
    {
        private readonly IPasswordHasher<Employee> _passwordHasher;
        private readonly IAuthService _authService;

        public AuthController(IPasswordHasher<Employee> passwordHasher, IAuthService authService)
        {
            _passwordHasher = passwordHasher;
            _authService = authService;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerModel);
            }
            
            return Redirect(nameof(Login));
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
