using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TimeTable.Data;
using TimeTable.Filters;
using TimeTable.Models;

namespace TimeTable.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Department
        //[AuthorizeRole(0)]
        [AuthorizeRole(0)]

        public async Task<IActionResult> Index(string code, string name, int page = 1, int limit = 10)
        {
            // Apply filters and sorting
            var departmentsQuery = _context.Departments
                .OrderByDescending(d => d.Id) // Sort by latest first
                .AsQueryable();

            // Filter by Code if provided
            if (!string.IsNullOrEmpty(code))
            {
                departmentsQuery = departmentsQuery.Where(d => d.Code.Contains(code));
            }

            // Filter by Name if provided
            if (!string.IsNullOrEmpty(name))
            {
                departmentsQuery = departmentsQuery.Where(d => d.Name.Contains(name));
            }

            // Pagination logic
            var totalDepartments = await departmentsQuery.CountAsync();
            var departments = await departmentsQuery
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            // Create a ViewModel to pass data to the view
            var viewModel = new DepartmentIndexViewModel
            {
                Departments = departments,
                Page = page,
                Limit = limit,
                TotalCount = totalDepartments,
                CodeFilter = code,
                NameFilter = name
            };

            return View(viewModel);
        }


        // GET: Department/Create
        public IActionResult Create()
        {
            return PartialView("_Create");
        }

        // POST: Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Department created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return PartialView("_Create", department);
        }

        // GET: Department/Edit/{id}
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            return PartialView("_Edit", department);
        }

        // POST: Department/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Department department)
        {
            if (id != department.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Department updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.Id))
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
            return PartialView("_Edit", department);
        }

        // GET: Department/Delete/{id}
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return PartialView("_Delete", department);
        }

        // POST: Department/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Department deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
    }
}
