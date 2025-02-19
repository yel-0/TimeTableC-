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

       
    }
}
