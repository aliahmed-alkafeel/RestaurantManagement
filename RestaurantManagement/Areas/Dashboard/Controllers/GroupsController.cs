using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Areas.Dashboard.IServices;
using RestaurantManagement.Areas.Dashboard.ViewModels;
using System.Security.Claims;

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
        
        [HttpPost("EditGroup/{id:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGroup(GroupViewModel model)
        {
            var groupVm = await groupsService.UpdateGroupAsync(model,Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            return RedirectToAction(nameof(Groups));
        }
    }
}
