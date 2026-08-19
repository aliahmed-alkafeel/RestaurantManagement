using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Data;
using RestaurantManagement.IRepositories;
using RestaurantManagement.Models;

namespace RestaurantManagement.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<List<Employee>> GetAllEmployeesWithGroupsAsync()
        {
            return await _dbSet.Include(e => e.Group).ToListAsync();
        }
        public async Task<Employee?> GetEmployeeByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<Employee?> GetEmployeeByUsernameAsync(string username)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Username == username);
        }

        public async Task<Employee> GetEmployeeWithGroupAsync(Guid id)
        {
            var emp = await _dbSet.Include(e => e.Group).FirstOrDefaultAsync(e => e.Id == id);
            if (emp is null) throw new KeyNotFoundException("The Group Of the User is Deleted!");
            return emp;
        }
        public async Task<Employee> GetEmployeeWithGroupByUsernameAsync(string username)
        {
            var emp = await _dbSet.Include(e => e.Group).FirstOrDefaultAsync(e => e.Username == username);
            if (emp is null) throw new KeyNotFoundException("The Group Of the User is Deleted!");
            return emp;
        }

        public void Terminate(Employee employee, Guid createdById, CancellationToken cancellationToken = default)
        {
            employee.EmployeeEndingDate = DateTime.UtcNow;
            employee.IsDeleted = true;
            employee.DeletedAt = DateTime.UtcNow;
            employee.DeletedById = createdById;
            _dbSet.Update(employee);
        
    }
    }
}
