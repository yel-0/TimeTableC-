using Microsoft.EntityFrameworkCore;
using TimeTable.Data;

var builder = WebApplication.CreateBuilder(args);

// Add logging to the console
builder.Logging.AddConsole();  // Good for development. Consider other logging methods for production.

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext and connect to the SQL Server database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())  // Only in production
{
    app.UseExceptionHandler("/Home/Error");  // Custom error page
    app.UseHsts();  // HTTP Strict Transport Security (HSTS)
}

app.UseHttpsRedirection();  // Redirect HTTP requests to HTTPS
app.UseStaticFiles();  // Serve static files like images, CSS, JavaScript, etc.

app.UseRouting();  // Enable routing for controllers and views

app.UseAuthorization();  // Enable authorization middleware (for securing resources)

app.MapControllerRoute(
    name: "default",  // Default routing configuration
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();  // Run the app
