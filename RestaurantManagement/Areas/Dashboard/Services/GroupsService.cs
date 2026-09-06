using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Areas.Dashboard.IServices;
using RestaurantManagement.Areas.Dashboard.ViewModels;
using RestaurantManagement.IRepositories;
using RestaurantManagement.Models;
using RestaurantManagement.Repositories;

namespace RestaurantManagement.Areas.Dashboard.Services
{
    public class GroupsService(IUnitOfWork unitOfWork) : IGroupsService
    {
        public async Task<bool> CreateGroupAsync(GroupViewModel model)
        {
            if (model is null) throw new ArgumentNullException();
            bool hasRepeatedName = await unitOfWork.Groups.Select().Where(g => g.GroupName == model.GroupName).AnyAsync();
            if (hasRepeatedName) return false;
            Group group = new Group
            {
                Id = Guid.NewGuid(),
                GroupName = model.GroupName
            };
            var roles = await unitOfWork.Roles.GetRolesByNamesAsync(model.Roles);
            foreach (var role in roles)
            {
                group.GroupRoles.Add(new GroupRole
                {
                    GroupId = group.Id,
                    RoleId = role.Id
                });
            }
            await unitOfWork.Groups.AddAsync(group);
            await unitOfWork.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteGroupAsync(Guid modelId, Guid ModifierId)
        {
            var group = await unitOfWork.Groups.GetByIdAsync(modelId);
            if (group is null) throw new InvalidOperationException("There is no such group");
            unitOfWork.Groups.Delete(group, ModifierId);
            await unitOfWork.SaveChangesAsync();
            return true;
        }
        public async Task<List<GroupViewModel>> GetAllGroupsAsync()
        {
            var groups = await unitOfWork.Groups.GetAllGroupsWithRolesAsync();
            List<GroupViewModel> groupsVm = [];
            foreach (var group in groups)
            {
                groupsVm.Add(new GroupViewModel
                {
                    Id = group.Id,
                    GroupName = group.GroupName,
                    GroupRoles = group.GroupRoles
                });
            }
            return (groupsVm);
        }

        public async Task<GroupViewModel> GetGroupByIdAsync(Guid id)
        {
            var group = await unitOfWork.Groups.GetGroupWithRolesByIdAsync(id);
            GroupViewModel groupVm = new GroupViewModel
            {
                Id = group.Id,
                GroupName = group.GroupName,
                GroupRoles = group.GroupRoles
            };
            return (groupVm);
        }

        public async Task<bool> UpdateGroupAsync(GroupViewModel model, Guid ModifierId)
        {
            if (model is null) throw new ArgumentNullException();
            var group = await unitOfWork.Groups.GetByIdAsync(model.Id);
            if (group is null) throw new KeyNotFoundException("There is no such group");
            group.GroupName = model.GroupName;
            var roles = await unitOfWork.Roles.GetRolesByNamesAsync(model.Roles);
            await unitOfWork.GroupsRoles.DeleteByGroupIdAsync(model.Id);
            foreach(var role in roles)
            {
                group.GroupRoles.Add(new GroupRole
                {
                    GroupId = group.Id,
                    RoleId = role.Id
                });
            }
            unitOfWork.Groups.Update(group, ModifierId);
            await unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
