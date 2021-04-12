using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QASite.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using QASite.Data;

namespace QASite.Web.Controllers
{
    public class HomeController : Controller
    {
        readonly string _connectionString;
        public HomeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }
       


        public IActionResult Index()
        {
            var repo = new QuestionsRepository(_connectionString);

            var questions = repo.GetAllQuestions();
            var vm = new IndexViewModel {Questions = questions};
            return View(vm);
        }

        public IActionResult Privacy()
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
