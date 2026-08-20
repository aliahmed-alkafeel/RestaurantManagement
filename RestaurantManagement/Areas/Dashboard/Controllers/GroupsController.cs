using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Areas.Dashboard.IServices;
using RestaurantManagement.Areas.Dashboard.ViewModels;

namespace RestaurantManagement.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Route("[area]/[controller]")]
    [Authorize]
    public class GroupsController(IGroupsService groupsService) : Controller
    {
        [HttpGet("")]
        public async Task<IActionResult> Groups()
        {
            var groupsVm = await groupsService.GetAllGroupsAsync();
            return View(groupsVm);
        }
        [HttpGet("EditGroup/{id:guid}")]
        public async Task<IActionResult> EditGroup(Guid id)
        {
            var groupVm = await groupsService.GetGroupByIdAsync(id);
            return View(groupVm);
        }
    }
}
