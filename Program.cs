using Microsoft.EntityFrameworkCore;
using TimeTable.Data;

var builder = WebApplication.CreateBuilder(args);

// Add logging to the console (Good for development, consider different logging methods for production)
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext and connect to the SQL Server database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add session services (if you need to store user sessions, e.g., for login)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true;  // Helps prevent XSS attacks
    options.Cookie.IsEssential = true; // Makes session cookie essential for app operation
});

// Add authentication services (if you're using cookies or other auth methods in the future)
builder.Services.AddAuthentication("Cookies")
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login";  // Specify the path for login page
        options.AccessDeniedPath = "/User/AccessDenied"; // Add an access denied path
    });

var app = builder.Build();

// Configure the HTTP request pipeline.

if (!app.Environment.IsDevelopment())  // Only in production
{
    app.UseExceptionHandler("/Home/Error");  // Custom error page
    app.UseHsts();  // HTTP Strict Transport Security (HSTS)
}
else
{
    app.UseDeveloperExceptionPage(); // Developer exception page for easier debugging in dev mode
}

app.UseHttpsRedirection();  // Redirect HTTP requests to HTTPS
app.UseStaticFiles();  // Serve static files like images, CSS, JavaScript, etc.

// Use session middleware (for storing session data)
app.UseSession();

// Enable routing for controllers and views
app.UseRouting();

// Enable authentication and authorization middleware (for securing resources)
app.UseAuthentication();  // Add this to handle authentication
app.UseAuthorization();   // Add this to handle authorization

// Map the default controller route (for Home and other controllers)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapFallbackToController("NotFound", "Home");  // If no route is matched, it redirects to the NotFound controller

app.Run();  // Run the app
