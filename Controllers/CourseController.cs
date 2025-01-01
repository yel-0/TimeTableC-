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

        public async Task<IActionResult> Index(string courseCode, string name, int? departmentId, int page = 1, int limit = 1)
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


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Department) // Ensure the department is included
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            // Prepare the ViewModel with course details and department options
            var viewModel = new CourseEditViewModel
            {
                Id = course.Id,
                Name = course.Name,
                CourseCode = course.CourseCode,
                DepartmentId = course.DepartmentId,
                DepartmentName = course.Department.Name // Get department name
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseEditViewModel viewModel)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            try
            {
                // Update course properties
                course.Name = viewModel.Name;
                course.CourseCode = viewModel.CourseCode;
                course.DepartmentId = viewModel.DepartmentId;

                // Save changes to the database
                _context.Courses.Update(course);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the course. Please try again later.");
                return View(viewModel);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var course = _context.Courses.Find(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                _context.SaveChanges();
                return RedirectToAction("Index"); // Redirect to the course list page
            }

            return NotFound(); // Handle case where course is not found
        }


    }
}
