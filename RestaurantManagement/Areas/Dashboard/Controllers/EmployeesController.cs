using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using RestaurantManagement.Areas.Dashboard.IServices;
using RestaurantManagement.Areas.Dashboard.Services;
using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.Extensions;
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
        public async Task<IActionResult> Employees(CancellationToken cancellationToken)
        {
            var emps = await employeesService.GetAllEmployeesAsync(cancellationToken);
            return View(emps);
        }

        [Authorize(Roles = nameof(UserRole.ManageEmployees))]
        [HttpGet("EditEmployee/{id:guid}")]
        public async Task<IActionResult> EditEmployee(Guid id,CancellationToken cancellationToken)
        {
            ViewBag.groups = await employeesService.ShowCreateEmployeeAsync(cancellationToken);
            var emp = await employeesService.GetUpdateEmployeeByIdAsync(id,cancellationToken);
            return View(emp);
        }
        [Authorize(Roles = nameof(UserRole.ManageEmployees))]
        [HttpPost("EditEmployee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEmployee(UpdateEmployeeViewModel model)
        {
            //var passwordIsEmpty =
            //    string.IsNullOrEmpty(model.Password) &&
            //    string.IsNullOrEmpty(model.ConfirmPassword);

            //if (passwordIsEmpty)
            //{
            //    ModelState.Remove(nameof(model.Password));
            //    ModelState.Remove(nameof(model.ConfirmPassword));
            //}

            if (!ModelState.IsValid)
            {
                ViewBag.groups = await employeesService
                    .ShowCreateEmployeeAsync(model.CancellationToken);

                return View(model);
            }

            var result = await employeesService.UpdateEmployeeAsync(model);

            if (!result)
            {
                ModelState.AddModelError("", "This update is not allowed");

                ViewBag.groups = await employeesService
                    .ShowCreateEmployeeAsync(model.CancellationToken);

                return View(model);
            }

            TempData.SuccessMessage(
                NotificationExtensions.ActionType.Update,
                NotificationExtensions.EntityType.Employee);

            return RedirectToAction(nameof(Employees));
        }
        [Authorize(Roles = nameof(UserRole.ManageEmployees))]
        [HttpGet("TerminateEmployee/{id:guid}")]
        public async Task<IActionResult> TerminateEmployee(Guid id, CancellationToken cancellationToken)
        {
            var emp = await employeesService.GetCreateEmployeeByIdAsync(id,cancellationToken);
            return View(emp);
        }
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UserRole.ManageEmployees))]
        [HttpPost("ConfirmedTerminateEmployee/{id:guid}")]
        public async Task<IActionResult> ConfirmedTerminateEmployee(Guid id,CancellationToken cancellationToken)
        {
            var result = await employeesService.TerminateEmployeeAsync(id,cancellationToken);

                if (!result)
                {
                    TempData.WarningMessage(
                        NotificationExtensions.ActionType.Terminate,
                        NotificationExtensions.EntityType.Employee);
                    return RedirectToAction(nameof(Employees));
            }
                TempData.SuccessMessage(
                    NotificationExtensions.ActionType.Terminate,
                    NotificationExtensions.EntityType.Employee);
            return RedirectToAction(nameof(Employees));
        } 
        [Authorize(Roles = nameof(UserRole.ManageEmployees))]
        [HttpGet("DeleteEmployee/{id:guid}")]
        public async Task<IActionResult> DeleteEmployee(Guid id, CancellationToken cancellationToken)
        {
            var emp = await employeesService.GetCreateEmployeeByIdAsync(id,cancellationToken);
            return View(emp);
        }
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UserRole.ManageEmployees))]
        [HttpPost("ConfirmedDeleteEmployee/{id:guid}")]
        public async Task<IActionResult> ConfirmedDeleteEmployee(Guid id, CancellationToken cancellationToken)
        {
            var result = await employeesService.DeleteEmployeeAsync(id,cancellationToken);
            if (!result)
            {
                TempData.WarningMessage(
                    NotificationExtensions.ActionType.Delete,
                    NotificationExtensions.EntityType.Employee);
                return RedirectToAction(nameof(Employees));
            }

            TempData.SuccessMessage(
                NotificationExtensions.ActionType.Delete,
                NotificationExtensions.EntityType.Employee);
            return RedirectToAction(nameof(Employees));
        }
        [Authorize(Roles = nameof(UserRole.ManageEmployees))]
        [HttpGet("CreateEmployee")]
        public async Task<IActionResult> CreateEmployee(CancellationToken cancellationToken)
        {

            ViewBag.groups = await employeesService.ShowCreateEmployeeAsync(cancellationToken);
            return View();
        }

        [Authorize(Roles = nameof(UserRole.ManageEmployees))]
        [HttpPost("CreateEmployee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.groups = await employeesService.ShowCreateEmployeeAsync(model.CancellationToken);
                return View(model);
            }
            var result = await employeesService.CreateEmployeeAsync(model);
            if(result is false)
            {
                ModelState.AddModelError("","The User is Registered");
                ViewBag.groups = await employeesService.ShowCreateEmployeeAsync(model.CancellationToken);
                return View(model);
            }
            TempData.SuccessMessage(
                NotificationExtensions.ActionType.Create,
                NotificationExtensions.EntityType.Employee);
            return RedirectToAction(nameof(Employees));
        }

    }
}
