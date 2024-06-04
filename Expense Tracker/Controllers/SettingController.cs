using Microsoft.AspNetCore.Mvc;

namespace Expense_Tracker.Controllers
{
    public class SettingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
