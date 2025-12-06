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

        // ========== SIGN UP ==========

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

        // ========== LOGIN ==========

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
            Response.Cookies.Append("LastUser", user.FullName);


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


        [HttpGet]
        public IActionResult ManageUsers(string search)
        {
            var users = _context.Users.ToList();

            if (!string.IsNullOrEmpty(search))
            {
                users = users
                    .Where(u =>
                        u.FullName.Contains(search) ||
                        u.Email.Contains(search) ||
                        u.Phone.Contains(search))
                    .ToList();
            }

            ViewBag.Search = search;

            return View(users);
        }


        [HttpGet]
        public IActionResult EditUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public IActionResult EditUser(int id, string fullName, string email, string phone)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            user.FullName = fullName;
            user.Email = email;
            user.Phone = phone;

            _context.SaveChanges();

            return RedirectToAction("ManageUsers");
        }


        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return RedirectToAction("ManageUsers");
        }


        [HttpPost]
        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("LastUser");
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
