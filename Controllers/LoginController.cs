using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTable.Data;
using TimeTable.ViewModels;

namespace TimeTable.Controllers
{
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
                // Fetch user from database based on email
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email);

                // Check if the user exists and if the password matches
                if (user != null && user.Password == model.Password)
                {
                    // For successful login, store the user's email in session
                    HttpContext.Session.SetString("UserEmail", user.Email);

                    // Redirect to the home page
                    return RedirectToAction("Index", "Home");
                }

                // Invalid login attempt
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
            }

            return View(model);
        }
    }
}
