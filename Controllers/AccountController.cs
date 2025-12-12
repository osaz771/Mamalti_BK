using Mamalti.Data;
using Mamalti.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Mamalti.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<ApplicationUser> _hasher = new PasswordHasher<ApplicationUser>();

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Helper: session check
        private bool IsLoggedIn()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail"));
        }

        private string NormalizeEmail(string email)
        {
            return (email ?? "").Trim().ToLower();
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
            fullName = (fullName ?? "").Trim();
            email = NormalizeEmail(email);
            phone = (phone ?? "").Trim();

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phone) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                ViewBag.Message = "Please fill in all fields";
                return View();
            }

            if (password != confirmPassword)
            {
                ViewBag.Message = "Passwords do not match";
                return View();
            }

            bool exists = _context.Users.Any(u => u.Email.ToLower() == email);
            if (exists)
            {
                ViewBag.Message = "Email already exists";
                return View();
            }

            var user = new ApplicationUser
            {
                FullName = fullName,
                Email = email,
                Phone = phone
            };

            // ✅ Hash password before saving
            user.Password = _hasher.HashPassword(user, password);

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
            email = NormalizeEmail(email);

            var user = _context.Users.FirstOrDefault(u => u.Email.ToLower() == email);
            if (user == null)
            {
                ViewBag.Message = "Invalid email or password";
                return View();
            }

            // ✅ Verify hashed password
            var result = _hasher.VerifyHashedPassword(user, user.Password, password);
            if (result == PasswordVerificationResult.Failed)
            {
                ViewBag.Message = "Invalid email or password";
                return View();
            }

            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserName", user.FullName);
            Response.Cookies.Append("LastUser", user.FullName);

            return RedirectToAction("Index", "Home");
        }

        // ========== FORGET PASSWORD ==========

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            ViewBag.Message = TempData["ResetMessage"];
            return View();
        }

        [HttpPost]
        public IActionResult ForgetPassword(string email, string code, string newPassword, string confirmPassword)
        {
            email = NormalizeEmail(email);

            var user = _context.Users.FirstOrDefault(u => u.Email.ToLower() == email);
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

                // ✅ Hash new password before saving
                user.Password = _hasher.HashPassword(user, newPassword);
                _context.SaveChanges();

                TempData["ResetMessage"] = "Password changed successfully. You can log in now.";
                return RedirectToAction("Login");
            }

            ViewBag.ShowResetForm = true;
            ViewBag.Email = email;
            return View();
        }

        // ========== MANAGE USERS (Protected) ==========

        [HttpGet]
        public IActionResult ManageUsers(string search)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login");

            var users = _context.Users.ToList();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim();
                users = users
                    .Where(u =>
                        (u.FullName != null && u.FullName.Contains(search)) ||
                        (u.Email != null && u.Email.Contains(search)) ||
                        (u.Phone != null && u.Phone.Contains(search)))
                    .ToList();
            }

            ViewBag.Search = search;
            return View(users);
        }

        [HttpGet]
        public IActionResult EditUser(int id)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login");

            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost]
        public IActionResult EditUser(int id, string fullName, string email, string phone)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login");

            fullName = (fullName ?? "").Trim();
            email = NormalizeEmail(email);
            phone = (phone ?? "").Trim();

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phone))
            {
                ViewBag.Message = "Please fill in all fields";
                var current = _context.Users.FirstOrDefault(u => u.Id == id);
                return View(current);
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();

            // ✅ منع تكرار الإيميل على مستخدم ثاني
            bool emailTaken = _context.Users.Any(u => u.Id != id && u.Email.ToLower() == email);
            if (emailTaken)
            {
                ViewBag.Message = "Email already exists";
                return View(user);
            }

            user.FullName = fullName;
            user.Email = email;
            user.Phone = phone;

            _context.SaveChanges();
            return RedirectToAction("ManageUsers");
        }

        // ✅ Delete should be POST
        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login");

            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            _context.SaveChanges();

            return RedirectToAction("ManageUsers");
        }

        // ========== LOGOUT ==========
        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("LastUser");
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
