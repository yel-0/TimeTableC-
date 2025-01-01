using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TimeTable.Data;
using TimeTable.Models;

namespace TimeTable.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string courseCode, string name, int? departmentId, int page = 1, int limit = 10)
        {
            // Apply filters and pagination
            var coursesQuery = _context.Courses.Include(c => c.Department).AsQueryable();

            // Filter by Course Code if provided
            if (!string.IsNullOrEmpty(courseCode))
            {
                coursesQuery = coursesQuery.Where(c => c.CourseCode.Contains(courseCode));
            }

            // Filter by Name if provided
            if (!string.IsNullOrEmpty(name))
            {
                coursesQuery = coursesQuery.Where(c => c.Name.Contains(name));
            }

            // Filter by Department if provided
            if (departmentId.HasValue)
            {
                coursesQuery = coursesQuery.Where(c => c.DepartmentId == departmentId.Value);
            }

            // Pagination logic
            var totalCourses = await coursesQuery.CountAsync();
            var courses = await coursesQuery
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            // Create a ViewModel to pass data to the view
            var viewModel = new CourseIndexViewModel
            {
                Courses = courses,
                Page = page,
                Limit = limit,
                TotalCount = totalCourses,
                CourseCodeFilter = courseCode,
                NameFilter = name,
                DepartmentIdFilter = departmentId
            };

            return View(viewModel);
        }


    }
}
