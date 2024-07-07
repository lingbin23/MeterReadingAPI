namespace MeterReadingAPI.Services
{
    public interface IMeterReadingService
    {
        Task<(int success, int failure)> ProcessMeterReadingsAsync(IFormFile file);
    }
}
