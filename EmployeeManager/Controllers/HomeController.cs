using Microsoft.AspNetCore.Mvc;

namespace EmployeeManager.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
