using MeterReadingAPI.Data;
using MeterReadingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MeterReadingAPI.Repositories
{
    public class MeterReadingRepository : IMeterReadingRepository
    {
        private readonly AppDbContext _context;

        public MeterReadingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddMeterReadingAsync(MeterReading meterReading)
        {
            _context.MeterReadings.Add(meterReading);
            await _context.SaveChangesAsync();
        }

        public async Task<MeterReading> GetLatestMeterReadingByAccountIdAsync(int accountId)
        {
            var theLatestReading = await _context.MeterReadings
                .Where(m => m.AccountId == accountId)
                .OrderByDescending(m => m.ReadingDate)
                .FirstOrDefaultAsync();

            //if (theLatestReading == null)
            //{
            //    throw new InvalidOperationException($"No meter reading found for account ID {accountId}");
            //}

            return theLatestReading;
        }
    }
}
