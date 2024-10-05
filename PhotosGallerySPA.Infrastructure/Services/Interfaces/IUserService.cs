using PhotosGallerySPA.Domain.Dtos.User;

namespace PhotosGallerySPA.Infrastructure.Services.Interfaces
{
    public interface IUserService
    {
        Task<(bool, string)> Login(LoginRegisterDto loginRegisterDto);

        Task<(bool, string)> Register(LoginRegisterDto loginRegisterDto);

        Task Logout();
    }
}
