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


        [HttpGet]
        public async Task<IActionResult> Create(string majorName = null, string facultyName = null, int majorPage = 1, int facultyPage = 1, int limit = 5)
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest") // Check if it's an AJAX request
            {
                // Searching Majors
                var majorsQuery = _context.Majors.AsQueryable();
                if (!string.IsNullOrEmpty(majorName))
                {
                    majorsQuery = majorsQuery.Where(m => m.Name.Contains(majorName));
                }

                // Ensure that Majors query is working correctly:
                var majors = await majorsQuery
                    .OrderBy(m => m.Name)
                    .Skip((majorPage - 1) * limit)
                    .Take(limit)
                    .ToListAsync();

                // Searching Faculties
                var facultiesQuery = _context.Faculties.AsQueryable();
                if (!string.IsNullOrEmpty(facultyName))
                {
                    facultiesQuery = facultiesQuery.Where(f => f.Name.Contains(facultyName));
                }

                // Ensure that Faculties query is working correctly:
                var faculties = await facultiesQuery
                    .OrderBy(f => f.Name)
                    .Skip((facultyPage - 1) * limit)
                    .Take(limit)
                    .ToListAsync();

                return Json(new { majors = majors, faculties = faculties });
            }

            // For regular page load (non-AJAX), return the view as usual
            var majorsList = await _context.Majors.Take(limit).ToListAsync();
            var facultiesList = await _context.Faculties.Take(limit).ToListAsync();

            var viewModel = new CourseCreateViewModel
            {
                Majors = majorsList,
                Faculties = facultiesList
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCourse(Course course)
        {
            try
            {
                var newCourse = new Course
                {
                    CourseCode = course.CourseCode,
                    Name = course.Name,
                    MajorId = course.MajorId,         // Only set the MajorId
                    FacultyId = course.FacultyId,     // Only set the FacultyId
                    Semester = course.Semester
                };

                // Add the new course to the database
                _context.Courses.Add(newCourse);
                await _context.SaveChangesAsync();

                // Return a success message
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the error (you can use a logger or any other method)
                // Example: _logger.LogError(ex, "Error while creating course");

                // Return an error message to the view or API
                ModelState.AddModelError("", "An error occurred while creating the course. Please try again later.");

                // Optionally, return the view with the current model data and errors
                return View(course);
            }
        }











    }
}
