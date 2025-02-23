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
    public class ClassroomController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClassroomController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Classroom
        [AuthorizeRole(0)]

        public async Task<IActionResult> Index(string name, int page = 1, int limit = 10)
        {
            // Default page and limit validation
            if (page < 1) page = 1;
            if (limit < 1) limit = 10;

            // Query the Classroom table, applying filtering and ordering by latest records first
            var query = _context.Classrooms
                .OrderByDescending(c => c.Id) // Order by latest records first
                .AsQueryable();

            // Filter by name if provided
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(c => c.Name.Contains(name));
            }

            // Get the total count of records (for pagination purposes)
            var totalCount = await query.CountAsync();

            // Apply pagination (skip and take)
            var classrooms = await query.Skip((page - 1) * limit).Take(limit).ToListAsync();

            // Create a ViewModel to send data to the view
            var viewModel = new ClassroomIndexViewModel
            {
                Classrooms = classrooms,
                TotalCount = totalCount,
                Page = page,
                Limit = limit,
                SearchTerm = name
            };

            return View(viewModel);
        }

        // GET: Classroom/Details/5

        public async Task<IActionResult> Details(int id)
        {
            var classroom = await _context.Classrooms.FindAsync(id);
            if (classroom == null)
            {
                return NotFound();
            }
            return View(classroom);
        }

        // GET: Classroom/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Classroom/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Classroom classroom)
        {
            if (ModelState.IsValid)
            {
                _context.Add(classroom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(classroom);
        }

        // GET: Classroom/Edit/5
     

        public async Task<IActionResult> Edit(int id)
        {
            var classroom = await _context.Classrooms.FindAsync(id);
            if (classroom == null)
            {
                return NotFound();
            }
            return View(classroom);
        }

        // POST: Classroom/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Classroom classroom)
        {
            if (id != classroom.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(classroom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Classrooms.Any(e => e.Id == classroom.Id))
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

            return View(classroom);
        }

        // GET: Classroom/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var classroom = await _context.Classrooms.FindAsync(id);
            if (classroom == null)
            {
                return NotFound();
            }
            return View(classroom);
        }

        // POST: Classroom/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var classroom = await _context.Classrooms.FindAsync(id);
            _context.Classrooms.Remove(classroom);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
