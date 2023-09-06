using Microsoft.AspNetCore.Mvc;
using ModernRecrut.MVC.Helpers;
using ModernRecrut.MVC.Models;
using System.Diagnostics;

namespace ModernRecrut.MVC.Controllers
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
        public IActionResult CodeStatus(int code)
        {
            if (code == 404)
            {
                return RedirectToAction("Erreur404", "Home");
            }

            CodeStatusViewModel codeStatusViewModel = new CodeStatusViewModel();

            codeStatusViewModel.IdRequete = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            codeStatusViewModel.CodeStatus = code;
            var message = CustomLogEvents.events.FirstOrDefault(x => x.Key == code).Value;
            if (message != null)
            {
                codeStatusViewModel.MessageErreur = message;
            }
            else
            {
                codeStatusViewModel.MessageErreur = "Erreur non recconue";
            }


            return View(codeStatusViewModel);
        }

        public IActionResult Erreur404()
        {
            return View();
        }
    }
}
