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
            var employee = await unitOfWork.Employees.NoTrackingSelect()
                .Include(e => e.Group)
                .ThenInclude(g => g!.GroupRoles.Where(gr => !gr.IsDeleted))
                .ThenInclude(gr => gr.Role)
                .Where(e => e.Group != null && !e.Group.IsDeleted)
                .FirstOrDefaultAsync(e => e.Id == empId);
            return View(employee);
        }
    }
}
