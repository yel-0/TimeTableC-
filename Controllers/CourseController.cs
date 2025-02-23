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
                NameFilter = name,
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
            // Check if CourseCode already exists
            if (_context.Courses.Any(c => c.CourseCode == model.CourseCode))
            {
                // Add a validation error to the model state
                ModelState.AddModelError("CourseCode", "Course Code must be unique.");
            }

            // Check if ShortTerm already exists
            if (!string.IsNullOrEmpty(model.ShortTerm) && _context.Courses.Any(c => c.ShortTerm == model.ShortTerm))
            {
                // Add a validation error to the model state
                ModelState.AddModelError("ShortTerm", "Short Term must be unique.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Create a new Course entity
            var course = new Course
            {
                CourseCode = model.CourseCode,
                Name = model.Name,
                ShortTerm = model.ShortTerm
            };

            // Add the new course to the context
            _context.Courses.Add(course);

            // Save the changes to the database
            await _context.SaveChangesAsync();

            // Redirect to the index or another action after successful creation
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
                CourseCode = course.CourseCode,
                ShortTerm =course.ShortTerm 
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

            // Check if the CourseCode is unique (excluding the current course)
            if (_context.Courses.Any(c => c.CourseCode == viewModel.CourseCode && c.Id != id))
            {
                // Return BadRequest with the validation message for CourseCode
                return BadRequest(new { Field = "CourseCode", Message = "Course Code must be unique." });
            }

            // Check if the ShortTerm is unique (excluding the current course)
            if (!string.IsNullOrEmpty(viewModel.ShortTerm) &&
                _context.Courses.Any(c => c.ShortTerm == viewModel.ShortTerm && c.Id != id))
            {
                // Return BadRequest with the validation message for ShortTerm
                return BadRequest(new { Field = "ShortTerm", Message = "Short Term must be unique." });
            }



            try
            {
                course.Name = viewModel.Name;
                course.CourseCode = viewModel.CourseCode;
                course.ShortTerm = viewModel.ShortTerm;

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