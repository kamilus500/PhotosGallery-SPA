using Microsoft.AspNetCore.Mvc;
using PhotosGallerySPA.MVC.Models;
using System.Diagnostics;

namespace PhotosGallerySPA.MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
            => View();

        public IActionResult _Index()
            => PartialView("Index");
    }
}
