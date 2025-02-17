using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTable.Data;
using TimeTable.Models;
using TimeTable.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using TimeTable.Filters;

namespace TimeTable.Controllers
{
    public class FacultyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FacultyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Faculty
        [AuthorizeRole(0)]

        public async Task<IActionResult> Index(string name, int page = 1, int limit = 10)
        {
            // Default page and limit values
            if (page < 1) page = 1;
            if (limit < 1) limit = 10;

            // Query the Faculty table, applying filtering and ordering
            var query = _context.Faculties
                .OrderByDescending(f => f.Id) // Sort latest first
                .AsQueryable();

            // Filter by name if provided
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(f => f.Name.Contains(name));
            }

            // Get the total count of records (for pagination purposes)
            var totalCount = await query.CountAsync();

            // Apply pagination (skip and take)
            var faculties = await query.Skip((page - 1) * limit).Take(limit).ToListAsync();

            // Create a ViewModel to send data to the view
            var viewModel = new FacultyIndexViewModel
            {
                Faculties = faculties,
                TotalCount = totalCount,
                Page = page,
                Limit = limit,
                SearchTerm = name
            };

            return View(viewModel);
        }


        // GET: Faculty/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var faculty = await _context.Faculties.FindAsync(id);
            if (faculty == null)
            {
                return NotFound();
            }
            return View(faculty);
        }

        // GET: Faculty/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Faculty/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                _context.Add(faculty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(faculty);
        }

        // GET: Faculty/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var faculty = await _context.Faculties.FindAsync(id);
            if (faculty == null)
            {
                return NotFound();
            }
            return View(faculty);
        }

        // POST: Faculty/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Faculty faculty)
        {
            if (id != faculty.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(faculty);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Faculties.Any(e => e.Id == faculty.Id))
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

            return View(faculty);
        }

        // GET: Faculty/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var faculty = await _context.Faculties.FindAsync(id);
            if (faculty == null)
            {
                return NotFound();
            }
            return View(faculty);
        }

        // GET: Faculty/Search
        public async Task<IActionResult> Search(string name, int limit = 10)
        {
            // Validate limit parameter
            if (limit < 1) limit = 10;

            // Query the Faculty table, applying filtering
            var query = _context.Faculties.AsQueryable();

            // Filter by name if provided
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(f => f.Name.Contains(name));
            }

            // Limit the number of results
            var faculties = await query.Take(limit).ToListAsync();

            // Create a ViewModel to send data to the view
            var viewModel = new FacultySearchViewModel
            {
                Faculties = faculties,
                SearchTerm = name,
                Limit = limit
            };

            return View(viewModel);
        }


        // POST: Faculty/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var faculty = await _context.Faculties.FindAsync(id);
            _context.Faculties.Remove(faculty);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
