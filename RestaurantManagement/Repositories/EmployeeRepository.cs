using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Data;
using RestaurantManagement.IRepositories;
using RestaurantManagement.Models;

namespace RestaurantManagement.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }
        public async Task<List<Employee>> GetAllEmployeesWithGroupsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.Include(e => e.Group).ToListAsync(cancellationToken);
        }
        public async Task<Employee?> GetEmployeeByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Email == email,cancellationToken);
        }

        public async Task<Employee?> GetEmployeeByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Username == username,cancellationToken);
        }

        public async Task<Employee> GetEmployeeWithGroupAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var emp = await _dbSet.Include(e => e.Group).FirstOrDefaultAsync(e => e.Id == id,cancellationToken);
            if (emp is null) throw new KeyNotFoundException("The Group Of the User is Deleted!");
            return emp;
        }
        public async Task<Employee> GetEmployeeWithGroupByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            var emp = await _dbSet.Include(e => e.Group).FirstOrDefaultAsync(e => e.Username == username, cancellationToken);
            if (emp is null) throw new KeyNotFoundException("The Group Of the User is Deleted!");
            return emp;
        }

        public void Terminate(Employee employee)
        {
            employee.EmployeeEndingDate = DateTime.UtcNow;
            employee.IsDeleted = true;
            employee.DeletedAt = DateTime.UtcNow;
            employee.DeletedById = userId;
            _dbSet.Update(employee);
        
    }
    }
}
