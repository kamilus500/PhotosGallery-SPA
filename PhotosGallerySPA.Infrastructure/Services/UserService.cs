﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PhotosGallerySPA.Domain.Dtos.User;
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
        public UserService(ApplicationDbContext dbContext, ISessionService sessionService, IEmailService emailService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        public async Task<bool> Login(LoginRegisterDto loginRegisterDto)
        {
            try
            {
                var user = await _dbContext.Users
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(x => x.Email == loginRegisterDto.Email);

                if (user == null)
                    throw new ArgumentNullException(nameof(user));

                if (!PasswordHelper.VerifyPassword(loginRegisterDto.Password, user.PasswordHashed))
                    return false;

                _sessionService.SetValue("UserId", user.Id);
                _sessionService.SetValue("UserFullName", $"{user.FirstName} {user.LastName}");

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task Logout()
        {
            _sessionService.Clear();
        }

        public async Task<bool> Register(LoginRegisterDto loginRegisterDto)
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
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
