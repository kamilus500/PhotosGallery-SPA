using Microsoft.AspNetCore.Mvc;
using PhotosGallerySPA.MVC.Models;
using System.Diagnostics;

namespace PhotosGallerySPA.MVC.Controllers
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

        public async Task<IActionResult> _Index()
        {
            return PartialView("Index");
        }

        public async Task<IActionResult> _Example()
        {
            return PartialView("_Example");
        }
    }
}
