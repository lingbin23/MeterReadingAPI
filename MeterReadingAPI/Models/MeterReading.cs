namespace MeterReadingAPI.Models
{
    public class MeterReading
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public DateTime ReadingDate { get; set; }
        public required string MeterReadValue { get; set; }
    }
}
