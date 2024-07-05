using Microsoft.AspNetCore.Mvc;
using PhotosGallerySPA.Domain.Dtos.Photo;
using PhotosGallerySPA.Infrastructure.CustomAttributes;
using PhotosGallerySPA.Infrastructure.Services.Interfaces;

namespace PhotosGallerySPA.MVC.Controllers
{
    public class PhotoController : Controller
    {
        private readonly IPhotoService _photoService;
        public PhotoController(IPhotoService photoService)
            => _photoService = photoService ?? throw new ArgumentNullException(nameof(photoService));

        [AuthorizationFilter]
        public async Task<IActionResult> _Photos()
            => PartialView("_Photos", await _photoService.GetPhotos());

        public IActionResult _CreatePhoto()
            => PartialView("_CreatePhoto");

        [HttpPost]
        public async Task<bool> CreatePhoto([FromForm] CreatePhotoDto createPhotoDto) 
            => await _photoService.CreatePhoto(createPhotoDto);

        [HttpPost]
        public async Task<bool> DeletePhoto([FromBody] string id)
            => await _photoService.DeletePhoto(id);
    }
}