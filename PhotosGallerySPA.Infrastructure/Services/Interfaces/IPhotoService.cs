using PhotosGallerySPA.Domain.Dtos.Photo;

namespace PhotosGallerySPA.Infrastructure.Services.Interfaces
{
    public interface IPhotoService
    {
        Task<IEnumerable<PhotoDto>> GetPhotos();

        Task<PhotoDto> GetPhoto(string id);

        Task CreatePhoto(CreatePhotoDto photo);

        Task DeletePhoto(string id);
    }
}
