using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace MeterReadingAPI.Controllers
{
    public class MeterReadingController : Controller
    {
        private readonly HttpClient _httpClient;

        public MeterReadingController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]  // Ensure this attribute is present
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Message = "Please select a CSV file.";
                return View();
            }

            var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(file.OpenReadStream())
            {
                Headers = { ContentType = new MediaTypeHeaderValue(file.ContentType) }
            };
            content.Add(fileContent, "file", file.FileName);

            var response = await _httpClient.PostAsync("https://localhost:5001/meter-reading-uploads", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                ViewBag.Message = $"File uploaded successfully. Result: {result}";
            }
            else
            {
                ViewBag.Message = "File upload failed.";
            }

            return View();
        }
    }
}
