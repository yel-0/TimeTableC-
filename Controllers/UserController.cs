using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TimeTable.Data;
using TimeTable.Models;
using TimeTable.ViewModels;

namespace TimeTable.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string name, Role? role, int page = 1, int limit = 2)
        {
            // Apply filters and pagination
            var usersQuery = _context.Users.AsQueryable();

            // Filter by Name if provided
            if (!string.IsNullOrEmpty(name))
            {
                usersQuery = usersQuery.Where(u => u.Name.Contains(name));
            }

            // Filter by Role if provided
            if (role.HasValue)
            {
                usersQuery = usersQuery.Where(u => u.Role == role.Value);
            }

            // Pagination logic
            var totalUsers = await usersQuery.CountAsync();
            var users = await usersQuery
                .Skip((page - 1) * limit).Take(limit).ToListAsync();

            // Create a ViewModel to pass data to the view
            var viewModel = new UserIndexViewModel
            {
                Users = users,
                Page = page,
                Limit = limit,
                TotalCount = totalUsers,
                NameFilter = name,
                RoleFilter = role
            };

            return View(viewModel);
        }


        //// GET: User/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Role,Email,Password")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return BadRequest(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }






        // Login POST action


    }
}
