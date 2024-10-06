using Microsoft.AspNetCore.Mvc;
using PhotosGallerySPA.Domain.Dtos.Global;
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
        [ValidateAntiForgeryToken]
        public async Task<ResultDto> Login([FromBody] LoginRegisterDto loginRegisterDto)
        {
            var (result, message) = await _userService.Login(loginRegisterDto);
            ModelState.Clear();
            return new ResultDto { Message = message, Result = result };
        }

        public IActionResult _Register()
            => PartialView("_Register");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ResultDto> Register([FromBody] LoginRegisterDto loginRegisterDto)
        {
            var (result, message) = await _userService.Register(loginRegisterDto);
            ModelState.Clear();
            return new ResultDto { Message = message, Result = result };
        }
    }
}
