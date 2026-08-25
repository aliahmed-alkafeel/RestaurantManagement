using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models;

namespace RestaurantManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { 
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<ItemOrder> ItemsOrders { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupRole> GroupsRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<GroupRole>().HasKey(gr => new { gr.GroupId, gr.RoleId });
            modelBuilder.Entity<ItemOrder>()
                .ToTable(obj => obj.HasCheckConstraint("CK_ItemOrder_Price_Positive", "Price >0"))
                .HasKey(it => new { it.OrderId, it.ItemId });
            modelBuilder.Entity<Item>()
                .ToTable(obj => obj.HasCheckConstraint("CK_Item_Price_Positive", "Price >0"));

            modelBuilder.Entity<Employee>().HasIndex(e => new { e.Email, e.IsDeleted, e.DeletedAt }).IsUnique();
            modelBuilder.Entity<Employee>().HasIndex(e => new { e.Username,e.IsDeleted, e.DeletedAt }).IsUnique();
            modelBuilder.Entity <Category>().HasIndex(c => new {c.CategoryName, c.Type, c.IsDeleted, c.DeletedAt}).IsUnique();

            modelBuilder.Entity<Item>().Property(i => i.Price).HasPrecision(18,5);
            modelBuilder.Entity<ItemOrder>().Property(i => i.Price).HasPrecision(18, 5);
            modelBuilder.Entity<Discount>().Property(d => d.DiscountPercentage).HasPrecision(3, 3);

        }
    }
}
