using Microsoft.AspNetCore.Mvc.Testing;
using MeterReadingAPI;
using Microsoft.Extensions.DependencyInjection;
using MeterReadingAPI.Data;
using Microsoft.EntityFrameworkCore;
//using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace MeterReadingAPI.Test
{
    [TestFixture]
    public class MeterReadingControllerTest
    {
        private HttpClient _client;
        private WebApplicationFactory<Program> _appFactory;

        [SetUp]
        public void SetUp()
        {
            // Initialize the WebApplicationFactory and HttpClient instance
            _appFactory = new WebApplicationFactory<Program>();
            _client = _appFactory.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            // Dispose of the HttpClient and WebApplicationFactory instances
            _client.Dispose();
            _appFactory.Dispose();
        }       

        [Test]
        public async Task PostMeterReading_ShouldReturnSuccess()
        {
            var formData = new MultipartFormDataContent();
            var filePath = @"C:\Users\jorda\Downloads\ENSEK Tech Exercise\Meter_Reading 2.csv"; // Ensure this path is correct
            var fileContent = new ByteArrayContent(File.ReadAllBytes(filePath));
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/csv");
            formData.Add(fileContent, "file", Path.GetFileName(filePath));

            var response = await _client.PostAsync("/meter-reading-uploads", formData);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

            // Verify data is saved
            using (var scope = _appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var meterReadingCount = await context.MeterReadings.CountAsync();
                Assert.Greater(meterReadingCount, 0);
            }
        }

    }
}