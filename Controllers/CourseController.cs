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

        public async Task<IActionResult> Index(string name, int? semester, int page = 1, int limit = 5)
        {
            var courses = _context.Courses.Include(c => c.Major).Include(c => c.Faculty).AsQueryable();

            // Apply filter by name if provided
            if (!string.IsNullOrEmpty(name))
            {
                courses = courses.Where(c => c.Name.Contains(name));
            }

            // Apply filter by semester if provided
            if (semester.HasValue)
            {
                courses = courses.Where(c => c.Semester == semester.Value);
            }

            var totalCourses = await courses.CountAsync();

            // Paginate the courses based on the current page and limit
            var coursesList = await courses
                .OrderBy(c => c.Name) // Sorting by name, you can change this to any other sorting criteria
                .Skip((page - 1) * limit) // Skip results for the current page
                .Take(limit) // Take only the number of results specified by the limit
                .ToListAsync();

            // Prepare the ViewModel with the courses and pagination data
            var viewModel = new CourseIndexViewModel
            {
                Courses = coursesList,
                TotalCourses = totalCourses,
                PageSize = limit,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalCourses / limit),
                NameFilter = name,
                SemesterFilter = semester
            };

            return View(viewModel);
        }

    }
}
