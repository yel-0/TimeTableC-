using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TimeTable.Data;
using TimeTable.ViewModels;

public class LoginController : Controller
{
    private readonly ApplicationDbContext _context;

    // Constructor to inject the database context
    public LoginController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Login
    public IActionResult Index()
    {
        return View();
    }

    // POST: Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Fetch user from the database based on email
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            // Check if the user exists and if the password matches
            if (user != null && user.Password == model.Password)
            {
                // Store the user's email and role (as integer) in the session
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetInt32("UserRole", (int)user.Role); // store role as int

                // Redirect to the home page or the page that the user should be taken to after login
                return RedirectToAction("Index", "Home");
            }

            // Invalid login attempt
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
        }

        return View(model);
    }

    public IActionResult Logout()
    {
        // Clear the user's session
        HttpContext.Session.Clear();

        // Optionally, show a toast message or log this action

        // Redirect to the Login page
        return RedirectToAction("Index", "Login");
    }
}
