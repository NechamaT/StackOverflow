using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using QASite.Data;
using QASite.Data.Migrations;
using QASite.Web.Models;

namespace QASite.Web.Controllers
{
    public class QuestionController : Controller
    {
        readonly string _connectionString;

        public QuestionController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Question q, List<string> tags)
        {
            var questionRepo = new QuestionsRepository(_connectionString);
            var usersRepo = new UsersRepository(_connectionString);
            q.User = usersRepo.GetUserByEmail(User.Identity.Name);
            q.DatePosted = DateTime.Now;
            questionRepo.AddQuestion(q, tags);
            return Redirect("Home/Index");
        }

        public IActionResult ViewQuestion(int id)
        {
            var connectionString = _connectionString;
            var repo = new QuestionsRepository(connectionString);
            var question = repo.GetQuestionById(id);
            var vm = new ViewQuestionViewModel()
                {
                    Question = question
                };
            return View(vm);
            }
        [Authorize]
        public IActionResult AddAnswer(Answer answer)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }

            var usersRepo = new UsersRepository(_connectionString);
            var questionRepo = new QuestionsRepository(_connectionString);
            answer.Date= DateTime.Now;
            answer.UserId = usersRepo.GetUserByEmail(User.Identity.Name).ID;
            questionRepo.AddAnswer(answer);
            return Redirect($"/question/viewquestion?id={answer.QuestionId}");
        }

        [HttpPost]
        public IActionResult Update(int id)
        {
            var questionRepo = new QuestionsRepository(_connectionString);
            var userRepo = new UsersRepository(_connectionString);
            var email = User.Identity.Name;
            var user = userRepo.GetUserByEmail(email);
            var question = questionRepo.GetQuestionById(id);
            if (question.Likes.Any((likes => likes.QuestionId == question.Id)))
            {
                return Redirect($"$/question/viewquestion?id={id}");
            }
            questionRepo.UpdateLike(id, user);
            return Json(id);
        }

        public IActionResult GetLikes(int id)
        {
            var repo = new QuestionsRepository(_connectionString);
            int likes = repo.GetLikes(id);
            return Json(likes);
        }

        }

}

