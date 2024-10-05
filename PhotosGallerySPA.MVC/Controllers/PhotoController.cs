using Microsoft.AspNetCore.Mvc;
using PhotosGallerySPA.Domain.Dtos.Global;
using PhotosGallerySPA.Domain.Dtos.Photo;
using PhotosGallerySPA.Infrastructure.CustomAttributes;
using PhotosGallerySPA.Infrastructure.Services.Interfaces;

namespace PhotosGallerySPA.MVC.Controllers
{
    public class PhotoController : Controller
    {
        private readonly IPhotoService _photoService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PhotoController(IPhotoService photoService, IWebHostEnvironment webHostEnvironment)
        {
            _photoService = photoService ?? throw new ArgumentNullException(nameof(photoService));
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
        }

        [AuthorizationFilter]
        public async Task<IActionResult> _Photos()
            => PartialView("_Photos", await _photoService.GetPhotos());

        public IActionResult _CreatePhoto()
            => PartialView("_CreatePhoto");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ResultDto> CreatePhoto([FromForm] CreatePhotoDto createPhotoDto)
        {
            var (result, message) = await _photoService.CreatePhoto(createPhotoDto, _webHostEnvironment.WebRootPath);
            return new ResultDto { Message = message, Result = result };
        }

        [HttpPost]
        public async Task<ResultDto> DeletePhoto([FromBody] string id)
        {
            var (result, message) = await _photoService.DeletePhoto(id, _webHostEnvironment.WebRootPath);
            return new ResultDto { Message = message, Result = result };
        }
    }
}