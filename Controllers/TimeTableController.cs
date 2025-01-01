using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TimeTable.Data;
using TimeTable.Models;
using TimeTable.ViewModels;

namespace TimeTable.Controllers
{
    public class TimeTableController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TimeTableController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string section, int? semester, int? year, string dayOfWeek, int? majorId, int page = 1, int limit = 10)
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
                query = query.Where(t => t.DayOfWeek.Equals(dayOfWeek, StringComparison.OrdinalIgnoreCase));
            }

            // Add filter for MajorId if provided
            if (majorId.HasValue)
            {
                query = query.Where(t => t.MajorId == majorId.Value);
            }

            // Get the total count of records for pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            var timetables = await query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

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
                MajorId = majorId // Pass MajorId to the view
            };

            return View(viewModel);
        }




        // GET: TimeTable/Create
        public IActionResult Create()
        {
            // Populate the dropdowns for Courses, Classrooms, Majors, and Faculties
            ViewData["Courses"] = _context.Courses.ToList();
            ViewData["Classrooms"] = _context.Classrooms.ToList();
            ViewData["Majors"] = _context.Majors.ToList(); // Assuming you have a Majors entity
            ViewData["Faculties"] = _context.Faculties.ToList(); // Assuming you have a Faculties entity
            return View();
        }


        // POST: TimeTable/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,ClassroomId,DayOfWeek,StartTime,EndTime,Year,Semester,Section,MajorId,FacultyId")] Timetable timetable)
        {
                _context.Add(timetable);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Redirect to the index page after saving
           
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var timetable = await _context.Timetables
                .Include(t => t.Course)
                .Include(t => t.Classroom)
                .Include(t => t.Faculty)
                .Include(t => t.Major)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (timetable == null)
            {
                return NotFound();
            }

            // Prepare the ViewModel for the edit view
            var viewModel = new TimeTableEditViewModel
            {
                Id = timetable.Id,
                CourseId = timetable.CourseId,
                CourseName = timetable.Course.Name,
                ClassroomId = timetable.ClassroomId,
                ClassroomName = timetable.Classroom.Name,
                FacultyId = timetable.FacultyId,
                FacultyName = timetable.Faculty.Name,
                StartTime = timetable.StartTime.ToString(@"hh\:mm"),  // Convert TimeSpan to HH:mm format
                EndTime = timetable.EndTime.ToString(@"hh\:mm"),      // Convert TimeSpan to HH:mm format
                DayOfWeek = timetable.DayOfWeek
            };

            return View(viewModel);
        }



        // POST: TimeTable/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Timetable timetable)
        {
            if (id != timetable.Id)
            {
                return NotFound();
            }

           
                try
                {
                    _context.Update(timetable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimetableExists(timetable.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));  // Redirect to index page after successful update
           // If validation fails, return the model back to the view
        }




        // POST: TimeTable/Delete/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int timetableId)  // Use 'timetableId' to match the form field name
        {
            // Find the timetable entry by ID
            var timetable = await _context.Timetables.FindAsync(timetableId);

            if (timetable == null)
            {
                // Handle case where the timetable entry doesn't exist
                return BadRequest(timetableId);// Return 404 if the timetable entry is not found
            }

            try
            {
                // Remove timetable from database
                _context.Timetables.Remove(timetable);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Return a bad request if there's an error during the deletion process
                return BadRequest("Error deleting the timetable entry: " + ex.Message);
            }

            // Redirect to Index page after successful deletion
            return RedirectToAction(nameof(Index));
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





        private bool TimetableExists(int id)
        {
            return _context.Timetables.Any(e => e.Id == id);
        }
    }
}
