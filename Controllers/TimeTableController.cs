using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TimeTable.Data;
using TimeTable.Models;

namespace TimeTable.Controllers
{
    public class TimeTableController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TimeTableController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TimeTable
        public async Task<IActionResult> Index(int? year, string dayOfWeek, int? semester)
        {
            var query = _context.Timetables
                .Include(t => t.Course)
                .Include(t => t.Classroom)
                .AsQueryable();

            // Apply year filter
            if (year.HasValue)
            {
                query = query.Where(t => t.Year == year.Value);
            }

            // Apply day of the week filter
            if (!string.IsNullOrEmpty(dayOfWeek))
            {
                query = query.Where(t => t.DayOfWeek == dayOfWeek);
            }

            // Apply semester filter through Course
            if (semester.HasValue)
            {
                query = query.Where(t => t.Course.Semester == semester.Value);
            }

            var timetables = await query.ToListAsync();
            return View(timetables);
        }

        // GET: TimeTable/Create
        public IActionResult Create()
        {
            // Populate the dropdowns for Courses and Classrooms, and add Semester
            ViewData["Courses"] = _context.Courses.ToList();
            ViewData["Classrooms"] = _context.Classrooms.ToList();
            return View();
        }

        // POST: TimeTable/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,ClassroomId,DayOfWeek,StartTime,EndTime,Year,Semester,Section")] Timetable timetable)
        {
          
                _context.Add(timetable);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Redirect to the index page after saving
           
        }

        // GET: TimeTable/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timetable = await _context.Timetables.FindAsync(id);
            if (timetable == null)
            {
                return NotFound();
            }

            // Populate the dropdowns for Courses and Classrooms
            ViewData["Courses"] = _context.Courses.ToList();
            ViewData["Classrooms"] = _context.Classrooms.ToList();
            return View(timetable);
        }

        // POST: TimeTable/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseId,ClassroomId,DayOfWeek,StartTime,EndTime,Year,Semester,Section")] Timetable timetable)
        {
            if (id != timetable.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
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
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdowns if the model is invalid
            ViewData["Courses"] = _context.Courses.ToList();
            ViewData["Classrooms"] = _context.Classrooms.ToList();
            return View(timetable);
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



        private bool TimetableExists(int id)
        {
            return _context.Timetables.Any(e => e.Id == id);
        }
    }
}
