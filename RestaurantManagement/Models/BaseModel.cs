namespace RestaurantManagement.Models
{
    public abstract class BaseModel
    {
        //CreatedUserId ?
        public Guid CreatedUserId { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; } = DateTime.MinValue;
        public Guid? DeletedById { get; set; }
        //public bool IsUpdated { get; set; }
        public DateTime? UpdatedAt { get; set; } = DateTime.MinValue;
        public Guid? UpdatedById { get; set; }
    }
}
