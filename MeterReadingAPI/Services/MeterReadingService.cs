using CsvHelper;
using CsvHelper.Configuration;
using MeterReadingAPI.Models;
using MeterReadingAPI.Repositories;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MeterReadingAPI.Services
{    
    public class MeterReadingService : IMeterReadingService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMeterReadingRepository _meterReadingRepository;
        private readonly ILogger<MeterReadingService> _logger;

        public MeterReadingService(ICustomerRepository customerRepository, IMeterReadingRepository meterReadingRepository, ILogger<MeterReadingService> logger)
        {
            _customerRepository = customerRepository;
            _meterReadingRepository = meterReadingRepository;
            _logger = logger;
        }

        public async Task<(int success, int failure)> ProcessMeterReadingsAsync(IFormFile file)
        {
            var successCount = 0;
            var failureCount = 0;

            using (var reader = new StreamReader(file.OpenReadStream()))

                try
                {
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        csv.Context.RegisterClassMap<MeterReadingMap>(); // Register the class map for MeterReadingDto

                        try
                        {
                            var records = csv.GetRecords<MeterReadingDto>(); // Read and parse all records in the CSV

                            foreach (var record in records)
                            {                                
                                var customer = await _customerRepository.GetCustomerByIdAsync(record.AccountId);

                                if (customer == null || !Regex.IsMatch(string.IsNullOrEmpty(record.MeterReadValue) ? string.Empty : record.MeterReadValue, @"^\d{5}$"))
                                {
                                    failureCount++;
                                    continue;
                                }

                                var existingRead = await _meterReadingRepository.GetLatestMeterReadingByAccountIdAsync(record.AccountId);
                                if (existingRead != null && existingRead.ReadingDate >= record.MeterReadingDateTime)
                                {
                                    failureCount++;
                                    continue;
                                }

                                var meterReading = new MeterReading
                                {
                                    AccountId = record.AccountId,
                                    ReadingDate = record.MeterReadingDateTime,
                                    MeterReadValue = string.IsNullOrEmpty(record.MeterReadValue) ? string.Empty : record.MeterReadValue
                                };

                                // Process valid record and save to database)
                                await _meterReadingRepository.AddMeterReadingAsync(meterReading);
                                _logger.LogInformation($"Processing valid data: AccountId={record.AccountId}, MeterReadingDateTime={record.MeterReadingDateTime}, MeterReadValue={record.MeterReadValue}");
                                successCount++;
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error processing meter readings from CSV");
                            throw; 
                        }
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while processing meter readings.");
                    throw;
                }

            return (successCount, failureCount);
        }

    }

    // DTO for CSV parsing
    public class MeterReadingDto
    {
        public int AccountId { get; set; }
        public DateTime MeterReadingDateTime { get; set; }
        public string? MeterReadValue { get; set; }
    }

    public class MeterReadingMap : ClassMap<MeterReadingDto>
    {
        public MeterReadingMap()
        {
            Map(m => m.AccountId).Name("AccountId");
            Map(m => m.MeterReadingDateTime).Name("MeterReadingDateTime").TypeConverterOption.Format("dd/MM/yyyy HH:mm");
            Map(m => m.MeterReadValue).Name("MeterReadValue");
        }
    }
}
