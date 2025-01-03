using Microsoft.AspNetCore.Mvc;

namespace TimeTable.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
