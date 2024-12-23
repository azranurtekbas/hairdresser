using haircare.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace haircare.Controllers
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


        public IActionResult about()
        {
            return View();
        }


        public IActionResult blog()
        {
            return View();
        }


        public IActionResult contact()
        {
            return View();
        }

        public IActionResult services()
        {
            return View();
        }

        public IActionResult GeriDon()
        {
            return RedirectToAction("Index");
        }

        public IActionResult galeri()
        {
            return View();
        }

        public IActionResult git()
        {
            return RedirectToAction("services");
        }

        public IActionResult admin_panel() 
        { 

         return View();

         }

        public IActionResult services1()
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
