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

        public async Task<IActionResult> Index(int? facultyId, int? majorId, string section, string semester, int? year, int page = 1, int limit = 10)
        {
            if (page < 1) page = 1;
            if (limit < 1) limit = 10;

            ViewBag.Majors = await _context.Majors.ToListAsync();

            if (!majorId.HasValue || string.IsNullOrWhiteSpace(section) || string.IsNullOrWhiteSpace(semester))
            {
                var emptyViewModel = new TimeTableAssignCourseIndexViewModel
                {
                    AssignCourses = new List<AssignCourse>(),
                    TotalAssignCount = 0,
                    TimetableEntries = new List<Timetable2>(),
                    TotalTimetableCount = 0,
                    CourseOccurrences = new Dictionary<int, CourseOccurrenceInfo>(),
                    Page = page,
                    Limit = limit,
                    FacultyId = facultyId,
                    MajorId = majorId,
                    Section = section,
                    Semester = semester,
                    Year = year
                };

                return View(emptyViewModel);
            }

            // Fetch assigned courses based on filters
            var assignQuery = _context.AssignCourses
                .Include(a => a.Faculty)
                .Include(a => a.Course)
                .Include(a => a.Major)
                .OrderByDescending(a => a.Id)
                .AsQueryable();

            if (facultyId.HasValue) assignQuery = assignQuery.Where(a => a.FacultyId == facultyId.Value);
            if (majorId.HasValue) assignQuery = assignQuery.Where(a => a.MajorId == majorId.Value);
            if (!string.IsNullOrWhiteSpace(section)) assignQuery = assignQuery.Where(a => a.Section.Contains(section));
            if (!string.IsNullOrWhiteSpace(semester)) assignQuery = assignQuery.Where(a => a.Semester == semester);
            if (year.HasValue) assignQuery = assignQuery.Where(a => a.Year == year.Value);

            var totalAssignCount = await assignQuery.CountAsync();
            var assignCourses = await assignQuery.Skip((page - 1) * limit).Take(limit).ToListAsync();

            assignCourses = assignCourses
            .OrderBy(a =>
                       int.TryParse(a.Course.CourseCode.Split('-').Last(), out int code) ? code : int.MaxValue
                   ).ToList();

            // Fetch timetable entries based on filters
            var timetableQuery = _context.Timetables2
                .Include(t => t.AssignCourse)
                .ThenInclude(ac => ac.Course)
                .Include(t => t.AssignCourse.Faculty)
                .Include(t => t.Classroom)
                .OrderBy(t => t.DayOfWeek)
                .AsQueryable();

            if (facultyId.HasValue) timetableQuery = timetableQuery.Where(t => t.AssignCourse.FacultyId == facultyId.Value);
            if (majorId.HasValue) timetableQuery = timetableQuery.Where(t => t.AssignCourse.MajorId == majorId.Value);
            if (!string.IsNullOrWhiteSpace(section)) timetableQuery = timetableQuery.Where(t => t.AssignCourse.Section.Contains(section));
            if (!string.IsNullOrWhiteSpace(semester)) timetableQuery = timetableQuery.Where(t => t.AssignCourse.Semester == semester);
            if (year.HasValue) timetableQuery = timetableQuery.Where(t => t.AssignCourse.Year == year.Value);

            var timetableEntries = await timetableQuery.ToListAsync();
            var totalTimetableCount = await timetableQuery.CountAsync();

            // Count occurrences of each Course in the timetable
            var courseOccurrences = await timetableQuery
                .GroupBy(t => t.AssignCourse.CourseId)
                .Select(g => new
                {
                    CourseId = g.Key,
                    Count = g.Count(),
                    CourseName = g.FirstOrDefault().AssignCourse.Course.Name,
                    CourseCode = g.FirstOrDefault().AssignCourse.Course.CourseCode
                })
                .ToListAsync();

            // Convert to dictionary
            var courseOccurrencesDictionary = courseOccurrences
                .ToDictionary(g => g.CourseId, g => new CourseOccurrenceInfo
                {
                    Count = g.Count,
                    CourseName = g.CourseName,
                    CourseCode = g.CourseCode
                });

            var viewModel = new TimeTableAssignCourseIndexViewModel
            {
                AssignCourses = assignCourses,
                TotalAssignCount = totalAssignCount,
                TimetableEntries = timetableEntries,
                TotalTimetableCount = totalTimetableCount,
                CourseOccurrences = courseOccurrencesDictionary,
                Page = page,
                Limit = limit,
                FacultyId = facultyId,
                MajorId = majorId,
                Section = section,
                Semester = semester,
                Year = year
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


        public async Task<IActionResult> FacultySearch(int? facultyId, int? year)
        {
            // Prepare the ViewModel for the data we need
            var viewModel = new ViewModels.TimeTableAssignCourseIndexViewModel
            {
                TimetableEntries = new List<Timetable2>(),
                TotalTimetableCount = 0
            };

            // If facultyId is not provided, return empty ViewModel (no data to show)
            if (!facultyId.HasValue)
            {
                return View(viewModel);
            }

            // Start building the query for the Timetable2 table with eager loading
            var timetableQuery = _context.Timetables2
                .Include(t => t.AssignCourse)
                .ThenInclude(ac => ac.Course)
                .Include(t => t.AssignCourse.Faculty)
                 .Include(t => t.AssignCourse.Major) // Include Major
                .Include(t => t.Classroom)
                .OrderBy(t => t.DayOfWeek) // Adjust ordering if necessary
                .AsQueryable();

            // Apply filters based on the provided facultyId and year
            if (facultyId.HasValue)
            {
                timetableQuery = timetableQuery.Where(t => t.AssignCourse.FacultyId == facultyId.Value);
            }

            if (year.HasValue)
            {
                timetableQuery = timetableQuery.Where(t => t.AssignCourse.Year == year.Value);
            }

            // Get the total count of records
            viewModel.TotalTimetableCount = await timetableQuery.CountAsync();

            // Retrieve the filtered timetable entries
            viewModel.TimetableEntries = await timetableQuery.ToListAsync();

            // Return the View with the updated ViewModel
            return View(viewModel);
        }





    }
}
