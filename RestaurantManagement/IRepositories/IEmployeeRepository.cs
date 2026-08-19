using RestaurantManagement.Models;

namespace RestaurantManagement.IRepositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee?> GetEmployeeByEmailAsync(string email);
        Task<List<Employee>> GetAllEmployeesWithGroupsAsync();
        Task<Employee> GetEmployeeWithGroupAsync(Guid id);
        Task<Employee> GetEmployeeWithGroupByUsernameAsync(string username);
        Task<Employee?> GetEmployeeByUsernameAsync(string username);
        void Terminate(Employee employee, Guid createdById, CancellationToken cancellationToken = default);

    }
}