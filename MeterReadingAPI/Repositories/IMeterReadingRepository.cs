using MeterReadingAPI.Models;

namespace MeterReadingAPI.Repositories
{
    // Repositories/IMeterReadingRepository.cs
    public interface IMeterReadingRepository
    {
        Task AddMeterReadingAsync(MeterReading meterReading);
        Task<MeterReading> GetLatestMeterReadingByAccountIdAsync(int accountId);
    }
}
