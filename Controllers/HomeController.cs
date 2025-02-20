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

        public IActionResult Index()
        {
            return View(); 
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
