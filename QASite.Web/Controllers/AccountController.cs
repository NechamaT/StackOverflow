using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using QASite.Data;

namespace QASite.Web.Controllers
{
    public class AccountController : Controller
    {
        readonly string _connectionString;
        public AccountController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup(User user, string password)
        {
            var repo = new UsersRepository(_connectionString);
            repo.AddUser(user, password);
            return Redirect("/account/login");
        }

        public IActionResult Login()
        {
            if (TempData["message"] != null)
            {
                ViewBag.Message = TempData["message"];
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var repo = new UsersRepository(_connectionString);
            var user = repo.LogIn(password, email);
            if (user == null)
            {
                TempData["message"] = "Invalid Login";
                return Redirect("/account/login");
            }

            var claims = new List<Claim>
            {
                new Claim("user", email) // this will get set to User.Identity.Name
            };
            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();

            return Redirect("/home/index");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/");
        }
    }
}
