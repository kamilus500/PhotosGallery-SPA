using Microsoft.AspNetCore.Mvc;

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
