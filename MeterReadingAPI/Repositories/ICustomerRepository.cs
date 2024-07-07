using MeterReadingAPI.Models;

namespace MeterReadingAPI.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> GetCustomerByIdAsync(int accountId);
        Task SeedCustomersAsync(IEnumerable<Customer> customers);
    }   
}
