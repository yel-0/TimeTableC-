using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTable.Data;
using TimeTable.Filters;
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
        [AuthorizeRole(0)]

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

            var orderedCourseOccurrences = courseOccurrences
    .OrderBy(c => int.TryParse(c.CourseCode.Split('-').Last(), out int code) ? code : int.MaxValue)
    .ToList();

            // Convert to dictionary
            var courseOccurrencesDictionary = orderedCourseOccurrences
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



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClassroomId,AssignCourseId,DayOfWeek,StartTime,EndTime")] Timetable2 timetable2, bool IsActive)
        {
            // Get the FacultyId based on AssignCourseId
            var assignCourse = await _context.AssignCourses.FindAsync(timetable2.AssignCourseId);
            if (assignCourse == null)
            {
                return BadRequest("Invalid course assignment.");
            }

            int facultyId = assignCourse.FacultyId;

            // Check for scheduling conflicts if IsActive is false
            if (!IsActive)
            {
                bool isConflict = await _context.Timetables2.AnyAsync(t =>
                    t.AssignCourse.FacultyId == facultyId && // Ensure the same faculty
                    t.DayOfWeek == timetable2.DayOfWeek && // Check the same day
                    ((t.StartTime <= timetable2.StartTime && t.EndTime > timetable2.StartTime) || // Overlapping start time
                     (t.StartTime < timetable2.EndTime && t.EndTime >= timetable2.EndTime) || // Overlapping end time
                     (t.StartTime >= timetable2.StartTime && t.EndTime <= timetable2.EndTime)) // Full overlap
                );

                if (isConflict)
                {
                    return BadRequest("The teacher is already scheduled to teach during this time on the same day.");
                }
            }

            _context.Add(timetable2);
            await _context.SaveChangesAsync();
            return Redirect(Request.Headers["Referer"].ToString());
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int timetableId)
        {
            // Find the timetable by its ID
            var timetable = _context.Timetables2.Find(timetableId);


            // Check if the timetable exists
            if (timetable != null)
            {
                // Remove the timetable from the database
                _context.Timetables2.Remove(timetable);
                _context.SaveChanges();

                // Redirect back to the previous page (or a specific action)
                return Redirect(Request.Headers["Referer"].ToString());
            }

            // Return NotFound if timetable does not exist
            return NotFound();
        }

        public async Task<JsonResult> ByFaculty(int facultyId)
        {
            var timetables = await _context.Timetables2
                .Include(t => t.AssignCourse)
                    .ThenInclude(ac => ac.Major)  // Include Major details
                .Include(t => t.AssignCourse)
                    .ThenInclude(ac => ac.Course)  // Include Course details
                .Where(t => t.AssignCourse.FacultyId == facultyId)
                .Select(t => new
                {
                    t.Id,
                    t.ClassroomId,
                    t.DayOfWeek,
                    StartTime = t.StartTime.ToString(@"hh\:mm"),
                    EndTime = t.EndTime.ToString(@"hh\:mm"),
                    AssignCourse = new
                    {
                        t.AssignCourse.Semester,
                        t.AssignCourse.Section,
                        Major = new
                        {
                            t.AssignCourse.Major.Id,
                            t.AssignCourse.Major.Name
                        },
                        Course = new
                        {
                            t.AssignCourse.Course.Id,
                            t.AssignCourse.Course.Name,
                            t.AssignCourse.Course.ShortTerm,
                            t.AssignCourse.Course.CourseCode

                        }
                    }
                })
                .ToListAsync();

            return Json(timetables);
        }




    }
}
