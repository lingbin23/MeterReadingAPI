using Microsoft.EntityFrameworkCore;
using MeterReadingAPI.Data;
using MeterReadingAPI.Repositories;
using MeterReadingAPI.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IMeterReadingRepository, MeterReadingRepository>();
builder.Services.AddScoped<IMeterReadingService, MeterReadingService>();
builder.Services.AddHttpClient();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});


builder.Services.AddControllers();
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
});

var app = builder.Build();
app.UseHttpsRedirection();

if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseHsts();
}
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization();

app.UseCors("AllowAllOrigins");
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Configure CORS
app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);

app.UseAntiforgery();

app.MapPost("/meter-reading-uploads", async (IFormFile file, [FromServices] IMeterReadingService meterReadingService) =>
{
    var result = await meterReadingService.ProcessMeterReadingsAsync(file);
    return Results.Ok(new { Success = result.success, Failure = result.failure });
});

app.Run();

public partial class Program { }