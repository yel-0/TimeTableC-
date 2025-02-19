using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTable.Data;
using TimeTable.Models;
using TimeTable.ViewModels;

namespace TimeTable.Controllers
{
    public class AssignCourseController : Controller
    {

        private readonly ApplicationDbContext _context;

        public AssignCourseController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? facultyId, int? courseId, string section, string semester, int? year, int page = 1, int limit = 10)
        {
            // Default page and limit validation
            if (page < 1) page = 1;
            if (limit < 1) limit = 10;

            // Start building the query for the AssignCourses table with eager loading
            var query = _context.AssignCourses
                .Include(a => a.Faculty)
                .Include(a => a.Course)
                .OrderByDescending(a => a.Id) // Order by latest records first
                .AsQueryable();

            // Apply filters based on provided parameters
            if (facultyId.HasValue)
            {
                query = query.Where(a => a.FacultyId == facultyId.Value);
            }

            if (courseId.HasValue)
            {
                query = query.Where(a => a.CourseId == courseId.Value);
            }

            if (!string.IsNullOrWhiteSpace(section))
            {
                query = query.Where(a => a.Section.Contains(section));
            }

            if (!string.IsNullOrWhiteSpace(semester))
            {
                query = query.Where(a => a.Semester == semester);
            }

            if (year.HasValue)
            {
                query = query.Where(a => a.Year == year.Value);
            }

            // Get the total count of records for pagination before applying Skip/Take
            var totalCount = await query.CountAsync();

            // Apply pagination after filters are applied
            var assignCourses = await query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            // Create a ViewModel to send data to the view
            var viewModel = new Models.AssignCourseIndexViewModel
            {
                AssignCourses = assignCourses,
                TotalCount = totalCount,
                Page = page,
                Limit = limit,
                FacultyId = facultyId,
                CourseId = courseId,
                Section = section,
                Semester = semester,
                Year = year
            };

            // Return the View with the ViewModel
            return View(viewModel);
        }


        public IActionResult Create()
        {
            var model = new AssignCourse(); 
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,Year,Semester,Section,FacultyId")] AssignCourse assignCourse)
        {

            _context.Add(assignCourse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: Edit
        public async Task<IActionResult> Edit(int id)
        {
            // Fetch the timetable record with related entities (Course and Faculty)
            var timetable = await _context.AssignCourses
                .Include(t => t.Course)
                .Include(t => t.Faculty)
                .FirstOrDefaultAsync(t => t.Id == id);

            // Check if timetable exists, if not, return NotFound
            if (timetable == null)
            {
                return NotFound();
            }

            // Prepare the view model with the timetable data
            var viewModel = new AssignCourseView
            {
                Id = timetable.Id,
                CourseId = timetable.CourseId,
                CourseName = timetable.Course.Name,
                FacultyId = timetable.FacultyId,
                FacultyName = timetable.Faculty.Name,
                Year = timetable.Year.ToString(),
                Semester = timetable.Semester,
                Section = timetable.Section,
            };

            // Return the view with the view model
            return View(viewModel);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseId,Year,Semester,Section,FacultyId")] AssignCourse assignCourse)
        {
            if (id != assignCourse.Id)
            {
                return NotFound();
            }

                    _context.Update(assignCourse);
                    await _context.SaveChangesAsync();
              
                return RedirectToAction(nameof(Index));
           
        }

        // Helper method to check if the assignment exists
        private bool AssignCourseExists(int id)
        {
            return _context.AssignCourses.Any(e => e.Id == id);
        }

    }
}
