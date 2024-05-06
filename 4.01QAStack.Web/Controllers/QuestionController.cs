using _4._01QAStack.Data;
using _4._01QAStack.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace _4._01QAStack.Web.Controllers
{
    public class QuestionController : Controller
    {
        private string _connectionString;
        public QuestionController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        [Authorize]
        public IActionResult AskAQuestion()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(Question question, List<string> tags)
        {
            var repo = new QuestionRepo(_connectionString);
            var userRepo = new UserRepo(_connectionString);
            question.Date = DateTime.Now;
            question.UserId = userRepo.GetByEmail(User.Identity.Name).Id;
            repo.AddQuestion(question, tags);
            return Redirect($"/question/viewQuestion?id={question.Id}");
        }

        public IActionResult ViewQuestion(int id)
        {
            var repo = new QuestionRepo(_connectionString);
            ViewQuestionViewModel vm = new();
            vm.Question = repo.GetQuestionById(id);
            return View(vm);
        }

        public IActionResult QuestionsByTag(string name)
        {
            var repo = new QuestionRepo(_connectionString);
            QuestionByTagViewModel vm = new();
            vm.Questions = repo.GetQuestionsForTag(name);
            return View(vm);
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddAnswer(Answer answer)
        {
            var repo = new UserRepo(_connectionString);
            var questionRepo = new QuestionRepo(_connectionString);
            answer.Date = DateTime.Now;
            answer.UserId = repo.GetByEmail(User.Identity.Name).Id;
            questionRepo.AddAnswer(answer);

            return Redirect($"/question/viewquestion?id={answer.QuestionId}");
        }


        [HttpPost]
        [Authorize]
        public void IncrementLikes(int id)
        {
            var ids = HttpContext.Session.Get<List<int>>("ids") ?? new List<int>();
            ids.Add(id);
            HttpContext.Session.Set("ids", ids);

            var repo = new QuestionRepo(_connectionString);
            repo.IncrementLikes(id);
        }

        public IActionResult IsLiked(int id)
        {
            var ids = HttpContext.Session.Get<List<int>>("ids");
            var liked = ids != null && ids.Contains(id);
            return Json(liked);
        }

        public IActionResult GetLikesById(int id)
        {
            var repo = new QuestionRepo(_connectionString);
            var likes = repo.GetLikesById(id);
            return Json(likes);
        }
    }
    
}

