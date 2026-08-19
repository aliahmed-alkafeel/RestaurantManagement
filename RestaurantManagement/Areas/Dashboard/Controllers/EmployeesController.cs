using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using RestaurantManagement.Areas.Dashboard.IServices;
using RestaurantManagement.Areas.Dashboard.ViewModels;
using System.Security.Claims;

namespace RestaurantManagement.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Route("[area]/[controller]")]
    [Authorize]
    public class EmployeesController(IEmployeesService employeesService) : Controller
    {
        [HttpGet(Name = "")]
        public async Task<IActionResult> Employees()
        {
            var emps = await employeesService.GetAllEmployeesAsync();
            return View(emps);
        }

        [HttpGet("EditEmployee/{id:guid}")]
        public async Task<IActionResult> EditEmployee(Guid id)
        {
            var emps = await employeesService.GetEmployeeByIdAsync(id);
            return View(emps);
        }
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
            await employeesService.UpdateEmployeeAsync(model, Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            return RedirectToAction(nameof(Employees));
        }

        [HttpGet("TerminateEmployee/{id:guid}")]
        public async Task<IActionResult> TerminateEmployee(Guid id)
        {
            var emps = await employeesService.GetEmployeeByIdAsync(id);
            return View(emps);
        }

        [HttpPost("ConfirmedTerminateEmployee/{id:guid}")]
        public async Task<IActionResult> ConfirmedTerminateEmployee(Guid id)
        {
            await employeesService.TerminateEmployeeAsync(id, Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            return RedirectToAction(nameof(Employees));
        }

        [HttpGet("CreateEmployee")]
        public async Task<IActionResult> CreateEmployee()
        {
            return View();
        }
        [HttpPost("CreateEmployee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmployee(ManageEmployeeViewModel model)
        {
            if (!ModelState.IsValid) return View(ModelState);
            var result = await employeesService.CreateEmployeeAsync(model);
            if(result is false)
            {
                ModelState.AddModelError("","The User is Regestered");
                return View(ModelState);
            } 
            return RedirectToAction(nameof(Employees));
        }

    }
}
