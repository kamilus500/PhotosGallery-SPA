using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PhotosGallerySPA.Domain.Dtos.User;
using PhotosGallerySPA.Domain.Entities;
using PhotosGallerySPA.Infrastructure.Extensions;
using PhotosGallerySPA.Infrastructure.Helpers;
using PhotosGallerySPA.Infrastructure.Persistance;
using PhotosGallerySPA.Infrastructure.Services.Interfaces;

namespace PhotosGallerySPA.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ISessionService _sessionService;
        private readonly IEmailService _emailService;
        private readonly IErrorService _errorService;
        public UserService(ApplicationDbContext dbContext, ISessionService sessionService, IEmailService emailService, IErrorService errorService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _errorService = errorService ?? throw new ArgumentNullException(nameof(errorService));
        }

        public async Task<(bool, string)> Login(LoginRegisterDto loginRegisterDto)
        {
            try
            {
                var user = await _dbContext.Users
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(x => x.Email == loginRegisterDto.Email);

                if (user == null)
                    return (false, "User with this email doesn't exist");

                if (!PasswordHelper.VerifyPassword(loginRegisterDto.Password, user.PasswordHashed))
                    return (false, "Password is not okey");

                _sessionService.SetValue("UserId", user.Id);
                _sessionService.SetValue("UserFullName", $"{user.FirstName} {user.LastName}");

                return (true, "Register successfully");
            }
            catch(Exception ex)
            {
                await _errorService.Create(new ErrorTable
                {
                    Id = Guid.NewGuid().ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    CreationDate = DateTimeProvider.DateNowUtc,
                    Exception = ex.Message.ToString()
                });
                return (false, "Something goes wrong");
            }
        }

        public async Task Logout()
        {
            _sessionService.Clear();
        }

        public async Task<(bool, string)> Register(LoginRegisterDto loginRegisterDto)
        {
            try
            {
                var user = loginRegisterDto.MapToUser();

                if (user == null)
                    throw new ArgumentNullException(nameof(user));

                user.PasswordHashed = PasswordHelper.HashPassword(loginRegisterDto.Password);

                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                await _emailService.Send(loginRegisterDto.Email, "Pomyślna rejestracja", EmailContents.ConfirmationMessage);

                return (true, "Register successfully");
            }
            catch(Exception ex)
            {
                await _errorService.Create(new ErrorTable
                {
                    Id = Guid.NewGuid().ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    CreationDate = DateTimeProvider.DateNowUtc,
                    Exception = ex.Message.ToString()
                });
                return (false, "Something goes wrong");
            }
        }
    }
}
