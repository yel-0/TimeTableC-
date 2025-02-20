using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TimeTable.Data;
using TimeTable.Filters;
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

        public async Task<IActionResult> Index(string courseCode, string name, int? majorId, int page = 1, int limit = 10)
        {
            var coursesQuery = _context.Courses
                .Include(c => c.Major)
                .OrderByDescending(c => c.Id)
                .AsQueryable();

            if (!string.IsNullOrEmpty(courseCode))
            {
                coursesQuery = coursesQuery.Where(c => c.CourseCode.Contains(courseCode));
            }

            if (!string.IsNullOrEmpty(name))
            {
                coursesQuery = coursesQuery.Where(c => c.Name.Contains(name));
            }

            if (majorId.HasValue)
            {
                coursesQuery = coursesQuery.Where(c => c.MajorId == majorId.Value);
            }

            var totalCourses = await coursesQuery.CountAsync();
            var courses = await coursesQuery
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            var viewModel = new CourseIndexViewModel
            {
                Courses = courses,
                Page = page,
                Limit = limit,
                TotalCount = totalCourses,
                CourseCodeFilter = courseCode,
                NameFilter = name,
                MajorIdFilter = majorId 
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create(string majorName = null, int majorPage = 1, int limit = 5)
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var majorsQuery = _context.Majors.AsQueryable();
                if (!string.IsNullOrEmpty(majorName))
                {
                    majorsQuery = majorsQuery.Where(m => m.Name.Contains(majorName));
                }

                var majors = await majorsQuery
                    .OrderBy(m => m.Name)
                    .Skip((majorPage - 1) * limit)
                    .Take(limit)
                    .ToListAsync();

                return Json(new { majors = majors });
            }

            var majorsList = await _context.Majors.Take(limit).ToListAsync();

            var viewModel = new CourseCreateViewModel
            {
                Majors = majorsList
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
                MajorId = model.MajorId
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Major)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            var viewModel = new CourseEditViewModel
            {
                Id = course.Id,
                Name = course.Name,
                CourseCode = course.CourseCode,
                MajorId = course.MajorId,
                MajorName = course.Major.Name
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
                course.Name = viewModel.Name;
                course.CourseCode = viewModel.CourseCode;
                course.MajorId = viewModel.MajorId;

                _context.Courses.Update(course);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while updating the course. Please try again later.");
                return View(viewModel);
            }
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult Delete(int id)
        {

            var course = _context.Courses.Find(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return NotFound();
        }
    }
}
