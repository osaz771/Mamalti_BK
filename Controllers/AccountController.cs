using Microsoft.AspNetCore.Mvc;

namespace Mamalti.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Signup()
        {
            return View();
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

    }
}
