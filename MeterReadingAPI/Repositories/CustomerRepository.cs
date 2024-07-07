using MeterReadingAPI.Data;
using MeterReadingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MeterReadingAPI.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> GetCustomerByIdAsync(int accountId)
        {
            return await _context.Customers.SingleOrDefaultAsync(c => c.AccountId == accountId);
        }

        public async Task SeedCustomersAsync(IEnumerable<Customer> customers)
        {
            _context.Customers.AddRange(customers);
            await _context.SaveChangesAsync();
        }
    }
}
