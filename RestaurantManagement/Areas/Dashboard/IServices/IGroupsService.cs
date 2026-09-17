using RestaurantManagement.Areas.Dashboard.ViewModels;

namespace RestaurantManagement.Areas.Dashboard.IServices
{
    public interface IGroupsService
    {
        Task<List<GroupViewModel>> GetAllGroupsAsync(CancellationToken cancellationToken = default);
        Task<GroupViewModel> GetGroupByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> UpdateGroupAsync(GroupViewModel model );
        Task<bool> CreateGroupAsync(GroupViewModel model);
        Task<bool> DeleteGroupAsync(Guid modelId, CancellationToken cancellationToken = default);
    }
}
