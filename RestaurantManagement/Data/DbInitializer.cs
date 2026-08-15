using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models;

namespace RestaurantManagement.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<Employee>>();
            
            var exsistingRoles = await context.Roles.Select(r => r.RoleName).ToListAsync();
            var rolesToAdd = Enum.GetValues<UserRole>().
                Where(r => r != UserRole.Unclassified && !exsistingRoles.Contains(r)).Select(r => new Role
                {
                    Id = new Guid(),
                    RoleName = r
                }).ToList();
            if (rolesToAdd.Count > 0)
            {
                await context.AddRangeAsync(rolesToAdd);
                await context.SaveChangesAsync();
            }


            var AdminGroup = await context.Groups.FirstOrDefaultAsync(g => g.GroupName == UserGroup.Administrator);
            if(AdminGroup is null)
            {
            AdminGroup = new Group
            {
                Id = new Guid(),
                GroupName = UserGroup.Administrator
            };
                await context.Groups.AddAsync(AdminGroup);
                await context.SaveChangesAsync();
            }

            var allRoles = await context.Roles.Where(r => r.RoleName != UserRole.Unclassified).ToListAsync();
            var existingRoleIds = await context.GroupsRoles.Where(gr => gr.GroupId == AdminGroup.Id)
                .Select(gr => gr.RoleId).ToListAsync();
            var groupRolesToAdd = allRoles.Where(r => !existingRoleIds.Contains(r.Id)).Select(r => new GroupRole
            {
                GroupId = AdminGroup.Id,
                RoleId = r.Id
            }).ToList();
            if(groupRolesToAdd.Count > 0)
            {
            await context.GroupsRoles.AddRangeAsync(groupRolesToAdd);
                await context.SaveChangesAsync();
            }
            var admin = await context.Employees.FirstOrDefaultAsync(e => e.Username == "admin");
            if (admin is null)
            {
            admin = new Employee
            {
                Id = new Guid(),
                GroupId = AdminGroup.Id,
                Username = "admin",
                Email = "ali3993100@gmail.com",
                FirstName = "System",
                LastName = "Admin",
                PhoneNumber = "02893928934",
                EmployeeStartingDate = DateTime.UtcNow,
                PasswordHash = ""
            };
            admin.PasswordHash = passwordHasher.HashPassword(admin, "adminpass");
            await context.Employees.AddAsync(admin);

            }
            else{ admin.IsUpdated = true; admin.UpdatedAt = DateTime.UtcNow; admin.UpdatedById = admin.Id; }
            
             
            await context.SaveChangesAsync();
        }
    }
}
