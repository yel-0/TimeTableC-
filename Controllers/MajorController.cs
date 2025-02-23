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
    public class MajorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MajorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Major
        //[AuthorizeRole(0)]
        [AuthorizeRole(0)]

        public async Task<IActionResult> Index(string name, int page = 1, int limit = 10)
        {
            // Default page and limit values
            if (page < 1) page = 1;
            if (limit < 1) limit = 10;

            // Query the majors table, applying filtering and ordering
            var query = _context.Majors
                .OrderByDescending(m => m.Id) // Sort latest first
                .AsQueryable();

            // Filter by name if provided
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(m => m.Name.Contains(name));
            }

            // Get the total count of records (for pagination purposes)
            var totalCount = await query.CountAsync();

            // Apply pagination (skip and take)
            var majors = await query.Skip((page - 1) * limit).Take(limit).ToListAsync();

            // Create a ViewModel to send data to the view
            var viewModel = new MajorIndexViewModel
            {
                Majors = majors,
                TotalCount = totalCount,
                Page = page,
                Limit = limit,
                NameSearch = name
            };

            return View(viewModel);
        }


        // GET: Major/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var major = await _context.Majors.FindAsync(id);

            if (major == null)
            {
                return NotFound();
            }

            return View(major);
        }

        // GET: Major/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Major/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Major major)
        {
            if (ModelState.IsValid)
            {
                // Add the new major to the context
                _context.Add(major);
                await _context.SaveChangesAsync();

                // Redirect back to the Index page to display the updated list of majors
                return RedirectToAction(nameof(Index));
            }

            // If the model is not valid, return the view with the error
            return View(major);
        }

        // GET: Major/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var major = await _context.Majors.FindAsync(id);

            if (major == null)
            {
                return NotFound();
            }

            return View(major);
        }

        // POST: Major/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Major major)
        {
            if (id != major.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(major);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MajorExists(major.Id))
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

            return View(major);
        }

        // GET: Major/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var major = await _context.Majors.FindAsync(id);
            if (major == null)
            {
                return NotFound();
            }

            return View(major);
        }

        // POST: Major/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var major = await _context.Majors.FindAsync(id);
            _context.Majors.Remove(major);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MajorExists(int id)
        {
            return _context.Majors.Any(e => e.Id == id);
        }
    }
}
