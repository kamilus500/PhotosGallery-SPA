using PhotosGallerySPA.Domain.Dtos.User;

namespace PhotosGallerySPA.Infrastructure.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> Login(LoginRegisterDto loginRegisterDto);

        Task<bool> Register(LoginRegisterDto loginRegisterDto);

        Task Logout();
    }
}
