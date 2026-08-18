using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.IServices;
using RestaurantManagement.ViewModels;

namespace RestaurantManagement.Controllers
{
    [Route("{Controller}/")]
    [Authorize]
    public class DashboardController(IDashboardService dashboardService) : Controller
    {
        [HttpGet("Employees")]
        public async Task<IActionResult> Employees()
        {
            var emps = await dashboardService.GetAllEmployeesAsync();
            return View(emps);
        }
        [HttpGet("Employees/Edit/{id:guid}")]
        public async Task<IActionResult> EditEmployee(Guid id)
        {
            var emps = await dashboardService.GetAllEmployeesAsync();
            return View(emps);
        }
        [HttpGet("Employees/Delete/{id:guid}")]
        public async Task<IActionResult> DeleteEmployee (Guid id)
        {
            var emps = await dashboardService.GetAllEmployeesAsync();
            return View(emps);
        }
        [HttpGet("Employees/Create/")]
        public async Task<IActionResult> CreateEmployee()
        {
            return View();
        }
        [HttpPost("Employees/Create/")]
        public async Task<IActionResult> CreateEmployee(ManageEmployeeViewModel manageEmp)
        {
            return RedirectToAction(nameof(Employees));
        }

    }
}
