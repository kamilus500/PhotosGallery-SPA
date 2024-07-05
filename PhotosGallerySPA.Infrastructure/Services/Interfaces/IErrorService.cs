using PhotosGallerySPA.Domain.Entities;

namespace PhotosGallerySPA.Infrastructure.Services.Interfaces
{
    public interface IErrorService
    {
        Task Create(ErrorTable error);
    }
}
