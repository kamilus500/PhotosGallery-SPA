using Microsoft.AspNetCore.Http;

namespace PhotosGallerySPA.Domain.Dtos.Photo
{
    public class CreatePhotoDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public string UserId { get; set; }
    }
}
