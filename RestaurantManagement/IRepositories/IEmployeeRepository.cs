using RestaurantManagement.Models;

namespace RestaurantManagement.IRepositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee?> GetEmployeeByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<List<Employee>> GetAllEmployeesWithGroupsAsync(CancellationToken cancellationToken = default);
        Task<Employee> GetEmployeeWithGroupAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Employee> GetEmployeeWithGroupByUsernameAsync(string username, CancellationToken cancellationToken = default);
        Task<Employee?> GetEmployeeByUsernameAsync(string username, CancellationToken cancellationToken = default);
        void Terminate(Employee employee);

    }
}