using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTable.Data;
using TimeTable.Filters;
using TimeTable.Models;
using TimeTable.ViewModels;

namespace TimeTable.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }


        [Route("NotFound")]
        public IActionResult NotFound()
        {
            return View(); // You can return a custom NotFound view
        }


        public async Task<IActionResult> Index(string section, int? semester, int? year, string dayOfWeek, int? majorId, int? facultyId, int page = 1, int limit = 10)
        {


            // Default page and limit validation
            if (page < 1) page = 1;
            if (limit < 1) limit = 10;

            // Query the Timetables table, applying filtering and eager loading
            var query = _context.Timetables
                .Include(t => t.Course)
                .Include(t => t.Classroom)
                .Include(t => t.Faculty)
                .Include(t => t.Major)
                .AsQueryable();

            // Apply filters based on provided parameters
            if (!string.IsNullOrWhiteSpace(section))
            {
                query = query.Where(t => t.Section.Contains(section));
            }

            if (semester.HasValue)
            {
                query = query.Where(t => t.Semester == semester.Value);
            }

            if (year.HasValue)
            {
                query = query.Where(t => t.Year == year.Value);
            }

            if (!string.IsNullOrWhiteSpace(dayOfWeek))
            {
                query = query.Where(t => t.DayOfWeek.ToLower() == dayOfWeek.ToLower());
            }


            // Filter by MajorId if provided
            if (majorId.HasValue)
            {
                query = query.Where(t => t.MajorId == majorId.Value);
            }

            // Filter by FacultyId if provided
            if (facultyId.HasValue)
            {
                query = query.Where(t => t.FacultyId == facultyId.Value);
            }

            // Get the total count of records for pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            var timetables = await query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            var userEmail = HttpContext.Session.GetString("UserEmail");


            // Create a ViewModel to send data to the view
            var viewModel = new TimetableIndexViewModel
            {
                Timetables = timetables,
                TotalCount = totalCount,
                Page = page,
                Limit = limit,
                Section = section,
                Semester = semester,
                Year = year,
                DayOfWeek = dayOfWeek,
                MajorId = majorId,
                FacultyId = facultyId // Pass FacultyId to the view
            };

            return View(viewModel);
        }




        [HttpGet]
        public async Task<IActionResult> GetDropDownData(
      string courseName = null,
      string classroomName = null,
      string majorName = null,
      string facultyName = null,
      int coursePage = 1,
      int classroomPage = 1,
      int majorPage = 1,
      int facultyPage = 1,
      int limit = 5)
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest") // Check if it's an AJAX request
            {
                // Initialize a dictionary to store the result sets
                var result = new Dictionary<string, object>
        {
            { "courses", new List<Course>() },
            { "classrooms", new List<Classroom>() },
            { "majors", new List<Major>() },
            { "faculties", new List<Faculty>() }
        };

                // Searching Courses
                if (!string.IsNullOrEmpty(courseName))
                {
                    var coursesQuery = _context.Courses.AsQueryable();
                    coursesQuery = coursesQuery.Where(c => c.Name.Contains(courseName));
                    result["courses"] = await coursesQuery
                        .OrderBy(c => c.Name)
                        .Skip((coursePage - 1) * limit)
                        .Take(limit)
                        .ToListAsync();
                }

                // Searching Classrooms
                if (!string.IsNullOrEmpty(classroomName))
                {
                    var classroomsQuery = _context.Classrooms.AsQueryable();
                    classroomsQuery = classroomsQuery.Where(c => c.Name.Contains(classroomName));
                    result["classrooms"] = await classroomsQuery
                        .OrderBy(c => c.Name)
                        .Skip((classroomPage - 1) * limit)
                        .Take(limit)
                        .ToListAsync();
                }

                // Searching Majors
                if (!string.IsNullOrEmpty(majorName))
                {
                    var majorsQuery = _context.Majors.AsQueryable();
                    majorsQuery = majorsQuery.Where(m => m.Name.Contains(majorName));
                    result["majors"] = await majorsQuery
                        .OrderBy(m => m.Name)
                        .Skip((majorPage - 1) * limit)
                        .Take(limit)
                        .ToListAsync();
                }

                // Searching Faculties
                if (!string.IsNullOrEmpty(facultyName))
                {
                    var facultiesQuery = _context.Faculties.AsQueryable();
                    facultiesQuery = facultiesQuery.Where(f => f.Name.Contains(facultyName));
                    result["faculties"] = await facultiesQuery
                        .OrderBy(f => f.Name)
                        .Skip((facultyPage - 1) * limit)
                        .Take(limit)
                        .ToListAsync();
                }

                return Json(result);
            }

            return BadRequest();
        }

    }
}
