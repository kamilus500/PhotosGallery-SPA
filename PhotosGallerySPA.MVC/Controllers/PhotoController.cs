using Microsoft.AspNetCore.Mvc;
using PhotosGallerySPA.Infrastructure.CustomAttributes;
using PhotosGallerySPA.Infrastructure.Services.Interfaces;

namespace PhotosGallerySPA.MVC.Controllers
{
    public class PhotoController : Controller
    {
        private readonly IPhotoService _photoService;
        public PhotoController(IPhotoService photoService)
        {
            _photoService = photoService ?? throw new ArgumentNullException(nameof(photoService));
        }

        [AuthorizationFilter]
        public async Task<IActionResult> Photos()
            => PartialView("_Photos", await _photoService.GetPhotos());
    }
}
