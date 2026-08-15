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
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var success = await _authService.LoginAsync(model);
            if (!success)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
