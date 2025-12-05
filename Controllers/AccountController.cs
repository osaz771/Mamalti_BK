using Mamalti.Data;
using Mamalti.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Mamalti.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup(string fullName, string email, string phone, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                ViewBag.Message = "Passwords do not match";
                return View();
            }

            bool exists = _context.Users.Any(u => u.Email == email);
            if (exists)
            {
                ViewBag.Message = "Email already exists";
                return View();
            }

            var user = new ApplicationUser
            {
                FullName = fullName,
                Email = email,
                Phone = phone,
                Password = password
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            TempData["ResetMessage"] = $"Welcome {user.FullName}, you can now log in.";

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.InfoMessage = TempData["ResetMessage"];
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                ViewBag.Message = "Invalid email or password";
                return View();
            }

            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserName", user.FullName);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            ViewBag.Message = TempData["ResetMessage"];
            return View();
        }

        [HttpPost]
        public IActionResult ForgetPassword(string email, string code, string newPassword, string confirmPassword)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                ViewBag.Message = "E-mail not found";
                return View();
            }

            if (!string.IsNullOrEmpty(newPassword))
            {
                if (newPassword != confirmPassword)
                {
                    ViewBag.Message = "Passwords do not match";
                    ViewBag.ShowResetForm = true;
                    ViewBag.Email = email;
                    return View();
                }

                user.Password = newPassword;
                _context.SaveChanges();
                TempData["ResetMessage"] = "Password changed successfully. You can log in now.";
                return RedirectToAction("Login");
            }

            ViewBag.ShowResetForm = true;
            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

    }
}
