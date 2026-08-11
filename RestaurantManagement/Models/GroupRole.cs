namespace RestaurantManagement.Models
{
    public class GroupRole
    {
        public Guid RoleId { get; set; }
        public Guid GroupId { get; set; }
        public Group Group { get; set; } = null!;
        public Role Role { get; set; } = null!;
    }
}
