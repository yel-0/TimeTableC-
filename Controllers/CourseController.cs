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

        public async Task<IActionResult> Index(string courseCode, string name, int page = 1, int limit = 10)
        {
            var coursesQuery = _context.Courses
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
                NameFilter = name
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course model)
        {
            var course = new Course
            {
                CourseCode = model.CourseCode,
                Name = model.Name
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            var viewModel = new CourseEditViewModel
            {
                Id = course.Id,
                Name = course.Name,
                CourseCode = course.CourseCode
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