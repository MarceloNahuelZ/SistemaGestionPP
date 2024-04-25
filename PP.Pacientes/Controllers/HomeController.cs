using Microsoft.AspNetCore.Mvc;
using PP.Pacientes.Models;
using System.Diagnostics;




namespace PP.Pacientes.Controllers
{
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Print()
        {
            var pdfResult = new Rotativa.AspNetCore.ViewAsPdf("Index", new { nombre = "pedrigo" }) { FileName = "Test.pdf" };
            return pdfResult;
        }


    }
}
