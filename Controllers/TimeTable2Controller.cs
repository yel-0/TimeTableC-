using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTable.Data;
using TimeTable.Models;
using TimeTable.ViewModels;

namespace TimeTable.Controllers
{
    public class TimeTable2Controller : Controller
    {
        private readonly ApplicationDbContext _context;

        public TimeTable2Controller(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? facultyId, int? majorId, string section, string semester, int? year, int page = 1, int limit = 10)
        {
            // Default page and limit validation
            if (page < 1) page = 1;
            if (limit < 1) limit = 10;

            //if (!facultyId.HasValue && (!majorId.HasValue || string.IsNullOrWhiteSpace(section) || string.IsNullOrWhiteSpace(semester)))
            //{
            //    var emptyViewModel = new ViewModels.TimeTableAssignCourseIndexViewModel
            //    {
            //        AssignCourses = new List<AssignCourse>(),
            //        TotalAssignCount = 0,
            //        TimetableEntries = new List<Timetable2>(),
            //        TotalTimetableCount = 0,
            //        Page = page,
            //        Limit = limit,
            //        FacultyId = facultyId,
            //        MajorId = majorId,
            //        Section = section,
            //        Semester = semester,
            //        Year = year
            //    };

            //    return View(emptyViewModel);
            //}

            // Start building the query for the AssignCourses table with eager loading
            var assignQuery = _context.AssignCourses
                .Include(a => a.Faculty)
                .Include(a => a.Course) // Include Course in AssignCourse
                .Include(a => a.Major)  // Include Major
                .OrderByDescending(a => a.Id) // Order by latest records first
                .AsQueryable();

            // Apply filters based on provided parameters for AssignCourses
            if (facultyId.HasValue)
            {
                assignQuery = assignQuery.Where(a => a.FacultyId == facultyId.Value);
            }

            if (majorId.HasValue)
            {
                assignQuery = assignQuery.Where(a => a.MajorId == majorId.Value);
            }

            if (!string.IsNullOrWhiteSpace(section))
            {
                assignQuery = assignQuery.Where(a => a.Section.Contains(section));
            }

            if (!string.IsNullOrWhiteSpace(semester))
            {
                assignQuery = assignQuery.Where(a => a.Semester == semester);
            }

            if (year.HasValue)
            {
                assignQuery = assignQuery.Where(a => a.Year == year.Value);
            }

            // Get the total count of records for pagination before applying Skip/Take for AssignCourses
            var totalAssignCount = await assignQuery.CountAsync();

            // Apply pagination after filters are applied for AssignCourses
            var assignCourses = await assignQuery
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            // Start building the query for the Timetable2 table with eager loading
            var timetableQuery = _context.Timetables2
                .Include(t => t.AssignCourse)
                .ThenInclude(ac => ac.Course)
                .Include(t => t.AssignCourse.Faculty)
                 .Include(t => t.Classroom)
                // Include Faculty through AssignCourse
                .OrderBy(t => t.DayOfWeek) // Adjust ordering if necessary
                .AsQueryable();

            // Apply filters based on provided parameters for Timetable2, using related AssignCourse properties
            if (facultyId.HasValue)
            {
                timetableQuery = timetableQuery.Where(t => t.AssignCourse.FacultyId == facultyId.Value);
            }

            if (majorId.HasValue)
            {
                timetableQuery = timetableQuery.Where(t => t.AssignCourse.MajorId == majorId.Value);
            }

            if (!string.IsNullOrWhiteSpace(section))
            {
                timetableQuery = timetableQuery.Where(t => t.AssignCourse.Section.Contains(section));
            }

            if (!string.IsNullOrWhiteSpace(semester))
            {
                timetableQuery = timetableQuery.Where(t => t.AssignCourse.Semester == semester);
            }

            if (year.HasValue)
            {
                timetableQuery = timetableQuery.Where(t => t.AssignCourse.Year == year.Value);
            }

            // Get the total count of records for pagination before applying Skip/Take for Timetable2
            var timetableEntries = await timetableQuery.ToListAsync();

            // Get the total count of records for pagination if needed
            var totalTimetableCount = await timetableQuery.CountAsync();

            // Create a ViewModel to send data to the view
            var viewModel = new ViewModels.TimeTableAssignCourseIndexViewModel
            {
                AssignCourses = assignCourses,
                TotalAssignCount = totalAssignCount,
                TimetableEntries = timetableEntries,
                TotalTimetableCount = totalTimetableCount,
                Page = page,
                Limit = limit,
                FacultyId = facultyId,
                MajorId = majorId,
                Section = section,
                Semester = semester,
                Year = year
            };

            // Return the View with the ViewModel
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClassroomId,AssignCourseId,DayOfWeek,StartTime,EndTime")] Timetable2 timetable2)
        {


            //if (ModelState.IsValid)
            
                _context.Add(timetable2);
                await _context.SaveChangesAsync();
            return Redirect(Request.Headers["Referer"].ToString());
            //return View(timetable2);
        }


    }
}
