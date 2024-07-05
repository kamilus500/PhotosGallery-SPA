using Microsoft.AspNetCore.Mvc;
using PhotosGallerySPA.Domain.Dtos.User;
using PhotosGallerySPA.Infrastructure.Services.Interfaces;

namespace PhotosGallerySPA.MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
            => _userService = userService ?? throw new ArgumentNullException(nameof(userService));

        public async Task<IActionResult> _Logout()
        {
            await _userService.Logout();
            return PartialView("~/Views/Home/Index.cshtml");
        }

        public IActionResult _Login()
            => PartialView("_Login");

        [HttpPost]
        public async Task<bool> Login([FromBody] LoginRegisterDto loginRegisterDto)
            => await _userService.Login(loginRegisterDto);

        public IActionResult _Register()
            => PartialView("_Register");

        [HttpPost]
        public async Task<bool> Register([FromBody] LoginRegisterDto loginRegisterDto)
            => await _userService.Register(loginRegisterDto);
    }
}
