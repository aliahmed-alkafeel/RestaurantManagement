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
            bool hasRepeatedName = await unitOfWork.Groups.Select().Where(g => g.GroupName == model.GroupName).AnyAsync(model.CancellationToken);
            if (hasRepeatedName) return false;
            Group group = new Group
            {
                Id = Guid.NewGuid(),
                GroupName = model.GroupName
            };
            var roles = await unitOfWork.Roles.GetRolesByNamesAsync(model.Roles, model.CancellationToken);
            foreach (var role in roles)
            {
                group.GroupRoles.Add(new GroupRole
                {
                    GroupId = group.Id,
                    RoleId = role.Id
                });
            }
            await unitOfWork.Groups.AddAsync(group, model.CancellationToken);
            await unitOfWork.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteGroupAsync(Guid modelId, CancellationToken cancellationToken = default)
        {
            var group = await unitOfWork.Groups.GetByIdAsync(modelId, cancellationToken);
            if (group is null) throw new InvalidOperationException("There is no such group");
            if (group.GroupName == InitUserGroup.Administrator.ToString()) return false;
            unitOfWork.Groups.Delete(group);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
        public async Task<List<GroupViewModel>> GetAllGroupsAsync(CancellationToken cancellationToken = default)
        {
            var groups = await unitOfWork.Groups.GetAllGroupsWithRolesAsync(cancellationToken);
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

        public async Task<GroupViewModel> GetGroupByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var group = await unitOfWork.Groups.GetGroupWithRolesByIdAsync(id, cancellationToken);
            GroupViewModel groupVm = new GroupViewModel
            {
                Id = group.Id,
                GroupName = group.GroupName,
                GroupRoles = group.GroupRoles
            };
            return (groupVm);
        }

        public async Task<bool> UpdateGroupAsync(GroupViewModel model)
        {
            if (model is null) throw new ArgumentNullException();
            var group = await unitOfWork.Groups.GetByIdAsync(model.Id, model.CancellationToken);
            if (group is null) throw new KeyNotFoundException("There is no such group");
            if (model.GroupName == InitUserGroup.Administrator.ToString() || group.GroupName == InitUserGroup.Administrator.ToString()) 
                return false;
            group.GroupName = model.GroupName;
            var roles = await unitOfWork.Roles.GetRolesByNamesAsync(model.Roles, model.CancellationToken);
            await unitOfWork.GroupsRoles.DeleteByGroupIdAsync(model.Id, model.CancellationToken);
            foreach(var role in roles)
            {
                group.GroupRoles.Add(new GroupRole
                {
                    GroupId = group.Id,
                    RoleId = role.Id
                });
            }
            unitOfWork.Groups.Update(group);
            await unitOfWork.SaveChangesAsync(model.CancellationToken);
            return true;
        }
    }
}
