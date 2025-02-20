using Microsoft.AspNetCore.Mvc;

namespace TimeTable.Controllers
{
    public class TimeTable2Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
