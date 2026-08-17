using RestaurantManagement.IRepositories;
using RestaurantManagement.IServices;
using RestaurantManagement.Repositories;

namespace RestaurantManagement.Services
{
    public class DashboardService<T> : IDashboardService<T>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DashboardService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task DeleteAsync(T t)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T t)
        {
            throw new NotImplementedException();
        }
    }
}
