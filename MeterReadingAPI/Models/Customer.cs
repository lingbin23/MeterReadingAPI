namespace MeterReadingAPI.Models
{
    // Models/Customer.cs
    public class Customer
    {
        public int AccountId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }   
}
