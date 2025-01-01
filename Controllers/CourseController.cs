using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TimeTable.Data;
using TimeTable.Models;
using TimeTable.ViewModels;

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

        [HttpGet]
        public async Task<IActionResult> Create(string departmentName = null, int departmentPage = 1, int limit = 5)
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest") // Check if it's an AJAX request
            {
                // Searching Departments
                var departmentsQuery = _context.Departments.AsQueryable();
                if (!string.IsNullOrEmpty(departmentName))
                {
                    departmentsQuery = departmentsQuery.Where(d => d.Name.Contains(departmentName));
                }

                var departments = await departmentsQuery
                    .OrderBy(d => d.Name)
                    .Skip((departmentPage - 1) * limit)
                    .Take(limit)
                    .ToListAsync();

                return Json(new { departments = departments });
            }

            // For regular page load (non-AJAX), return the view as usual
            var departmentsList = await _context.Departments.Take(limit).ToListAsync();

            var viewModel = new CourseCreateViewModel
            {
                Departments = departmentsList
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course model)
        {
        
                var course = new Course
                {
                    CourseCode = model.CourseCode,
                    Name = model.Name,
                    DepartmentId = model.DepartmentId
                };

                // Save course to the database
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();

                // Redirect to Index page after successful course creation
                return RedirectToAction("Index");
        
        }


    }
}
