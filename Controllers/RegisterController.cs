using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTable.Models;
using TimeTable.ViewModels;
using System.Threading.Tasks;
using TimeTable.Data;
using System;
using TimeTable.Filters;

namespace TimeTable.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor to inject the database context
        public RegisterController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Register
        //[AuthorizeRole(0)]

        public IActionResult Index()
        {
            return View(new RegisterViewModel()); // Initialize an empty model to display the registration form
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the email already exists in the database
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email);

                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Email is already taken.");
                    return View(model); // Return the view with error message if email exists
                }

                // Convert the role string to Role enum
                if (Enum.TryParse(model.Role, out Role role))
                {
                    // Create a new user object with the selected role
                    var user = new User
                    {
                        Name = model.Name,
                        Email = model.Email,
                        Password = model.Password, // Store the password as is (not hashed)
                        Role = role // Set the role from the registration form
                    };

                    // Save the new user to the database
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    // Redirect to the login page after successful registration
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid role selected.");
                    return View(model); // Return the view with error message if role is invalid
                }
            }

            // Return to the view with validation errors if the model is invalid
            return BadRequest(model);
        }
    }
}
