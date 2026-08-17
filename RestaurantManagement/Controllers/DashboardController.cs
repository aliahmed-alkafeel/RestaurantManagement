using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.IServices;

namespace RestaurantManagement.Controllers
{
    public class DashboardController(IDashboardService dashboardService) : Controller
    {
        public async Task<IActionResult> EmployeesAsync()
        {
            var emps = await dashboardService.GetAllEmployeesAsync();
            return View(emps);
        }
    }
}
