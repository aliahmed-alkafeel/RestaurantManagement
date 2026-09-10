using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.IRepositories;
using System.Security.Claims;

namespace RestaurantManagement.ViewComponents
{
    public class EmployeeProfileViewComponent(IUnitOfWork unitOfWork) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var empId = Guid.Parse(UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if(empId == Guid.Empty)
            {
                return View(null);
            }
            var employee = await unitOfWork.Employees.Select().Include(e => e.Group).ThenInclude(g => g.GroupRoles).ThenInclude(gr => gr.Role).AsNoTracking().FirstOrDefaultAsync(e => e.Id == empId);
            return View(employee);
        }
    }
}
