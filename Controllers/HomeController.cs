using System.Diagnostics;
using Mamalti.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;   // ✅ مهم للـ Session

namespace Mamalti.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // ✅ حماية صفحة الهوم: لو ما فيه مستخدم مسجل → يرجع لصفحة اللوقن
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
            {
                return RedirectToAction("Login", "Account");
            }

            // لو حاب تستخدم اسم المستخدم في الصفحة
            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
