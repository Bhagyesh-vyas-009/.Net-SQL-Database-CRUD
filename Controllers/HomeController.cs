using Microsoft.AspNetCore.Mvc;
using Coffee_Shop.Models;
using System.Diagnostics;
using Coffee_Shop.BAL;

namespace Coffee_Shop.Controllers
{
    [CheckAccess]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Department()
        {
            return View();
        }

        public IActionResult AddEditDepartment()
        {
            return View();
        }

        public IActionResult Employee()
        {
            return View();
        }

        public IActionResult AddEditEmployee()
        {
            return View();
        }

        public IActionResult Project()
        {
            return View();
        }

        public IActionResult AddEditProject()
        {
            return View();
        }

        public IActionResult EmployeeProject()
        {
            return View();
        }

        public IActionResult AddEditEmployeeProject()
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
