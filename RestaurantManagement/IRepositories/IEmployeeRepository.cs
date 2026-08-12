using RestaurantManagement.Models;

namespace RestaurantManagement.IRepositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee?> GetEmployeeByEmailAsync(string email);
        Task<Employee?> GetEmployeeByUsernameAsync(string username);
    }
}