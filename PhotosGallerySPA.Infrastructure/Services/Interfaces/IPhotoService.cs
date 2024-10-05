using PhotosGallerySPA.Domain.Dtos.Photo;

namespace PhotosGallerySPA.Infrastructure.Services.Interfaces
{
    public interface IPhotoService
    {
        Task<IEnumerable<PhotoDto>> GetPhotos();

        Task<PhotoDto> GetPhoto(string id);

        Task<(bool, string)> CreatePhoto(CreatePhotoDto photo, string rootPath);

        Task<(bool, string)> DeletePhoto(string id, string rootPath);
    }
}
