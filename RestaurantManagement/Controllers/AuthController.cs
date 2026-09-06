using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.IServices;
using RestaurantManagement.Models;
using RestaurantManagement.Services;
using RestaurantManagement.ViewModels;

namespace RestaurantManagement.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IPasswordHasher<Employee> passwordHasher, IAuthService authService)
        {
            _authService = authService;
        }
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid) return View(model);
            var success = await _authService.LoginAsync(model);
            if (!success)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(model);
            }
            if(!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("NewOrder", "POS");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LoginViewModel model)
        {
            await _authService.LogoutAsync();
            return RedirectToAction("NewOrder", "POS");
        }
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
