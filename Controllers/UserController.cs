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

        public async Task<IActionResult> Index(string name, Role? role, int page = 1, int limit = 10)
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
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

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

    


      

     
        // Login POST action
      

    }
}
