using Microsoft.AspNetCore.Mvc;
using odev.Models;
using System.Diagnostics;

namespace odev.Controllers
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

        public IActionResult Hizmetlerimiz()
        {

            return View();  
        }

        // Yeni bir Action: Geri D�n (Index'e y�nlendirme)
        public IActionResult GeriDon()
        {
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
