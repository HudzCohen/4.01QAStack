using _4._01QAStack.Data;
using _4._01QAStack.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace _4._01QAStack.Web.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString;

        public HomeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        public IActionResult Index()
        {
            var repo = new QuestionRepo(_connectionString);
            IndexViewModel vm = new();
            vm.Questions = repo.GetQuestions();
            return View(vm);
        }

    }
}
