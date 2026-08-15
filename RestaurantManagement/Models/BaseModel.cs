namespace RestaurantManagement.Models
{
    public abstract class BaseModel
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid? DeletedById { get; set; }
        public bool IsUpdated { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedById { get; set; }
    }
}
