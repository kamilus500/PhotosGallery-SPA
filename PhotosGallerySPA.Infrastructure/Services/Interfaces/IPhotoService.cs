using PhotosGallerySPA.Domain.Dtos.Photo;

namespace PhotosGallerySPA.Infrastructure.Services.Interfaces
{
    public interface IPhotoService
    {
        Task<IEnumerable<PhotoDto>> GetPhotos();

        Task<PhotoDto> GetPhoto(string id);

        Task<bool> CreatePhoto(CreatePhotoDto photo);

        Task<bool> DeletePhoto(string id);
    }
}
