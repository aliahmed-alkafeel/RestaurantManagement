using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using RestaurantManagement.Areas.Dashboard.IServices;
using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.Models;
using System.Security.Claims;

namespace RestaurantManagement.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Route("[area]/[controller]")]
    public class EmployeesController(IEmployeesService employeesService) : Controller
    {
        [Authorize(Roles = nameof(UserRole.AccessEmployees))]
        [HttpGet("")]
        public async Task<IActionResult> Employees()
        {
            var emps = await employeesService.GetAllEmployeesAsync();
            return View(emps);
        }

        [Authorize(Roles = nameof(UserRole.ManageEmployees))]
        [HttpGet("EditEmployee/{id:guid}")]
        public async Task<IActionResult> EditEmployee(Guid id)
        {
            var emps = await employeesService.GetEmployeeByIdAsync(id);
            return View(emps);
        }
        [Authorize(Roles = nameof(UserRole.ManageEmployees))]
        [HttpPost("EditEmployee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEmployee(ManageEmployeeViewModel model)
        {
            var cloendModelState = new ModelStateDictionary(ModelState);
                ModelState.Remove(nameof(model.Password));
                ModelState.Remove(nameof(model.ConfirmPassword));
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (!cloendModelState.IsValid && !string.IsNullOrEmpty(model.Password) && !string.IsNullOrEmpty(model.ConfirmPassword))
            {                             
                    return View(model);
            }
            var result = await employeesService.UpdateEmployeeAsync(model, Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if(!result)
            {
                ModelState.AddModelError("","This update is not allowed");
                return View(model);
            }
            return RedirectToAction(nameof(Employees));
        }

        [Authorize(Roles = nameof(UserRole.ManageEmployees))]
        [HttpGet("TerminateEmployee/{id:guid}")]
        public async Task<IActionResult> TerminateEmployee(Guid id)
        {
            var emps = await employeesService.GetEmployeeByIdAsync(id);
            return View(emps);
        }
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UserRole.ManageEmployees))]
        [HttpPost("ConfirmedTerminateEmployee/{id:guid}")]
        public async Task<IActionResult> ConfirmedTerminateEmployee(Guid id)
        {
            await employeesService.TerminateEmployeeAsync(id, Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            return RedirectToAction(nameof(Employees));
        }

        [Authorize(Roles = nameof(UserRole.ManageEmployees))]
        [HttpGet("CreateEmployee")]
        public async Task<IActionResult> CreateEmployee()
        {
            return View();
        }

        [Authorize(Roles = nameof(UserRole.ManageEmployees))]
        [HttpPost("CreateEmployee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmployee(ManageEmployeeViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await employeesService.CreateEmployeeAsync(model);
            if(result is false)
            {
                ModelState.AddModelError("","The User is Regestered");
                return View(model);
            } 
            return RedirectToAction(nameof(Employees));
        }

    }
}
