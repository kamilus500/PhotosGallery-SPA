﻿using PhotosGallerySPA.Domain.Dtos.Photo;
using PhotosGallerySPA.Domain.Dtos.User;
using PhotosGallerySPA.Domain.Entities;

namespace PhotosGallerySPA.Infrastructure.Extensions
{
    public static class MapperExtensions
    {
        public static List<PhotoDto> MapToPhotoDtoList(this List<Photo> photos)
            => photos.Select(photo => photo.MapToPhotoDto()).ToList();

        public static PhotoDto MapToPhotoDto(this Photo photo)
            => new PhotoDto()
            {
                Id = photo.Id,
                CreationDate = photo.CreationDate,
                Description = photo.Description,
                Title = photo.Title,
                Image = File.ReadAllBytes(photo.FileName),
                UserId = photo.UserId,
                FullName = $"{photo.User.FirstName} {photo.User.LastName}"
            };

        public static Photo MapToPhoto(this PhotoDto photoDto)
            => new Photo()
            {
                Id = photoDto.Id,
                CreationDate = photoDto.CreationDate,
                Description = photoDto.Description,
                Title = photoDto.Title,
                UserId = photoDto.UserId
            };

        public static User MapToUser(this LoginRegisterDto loginRegisterDto)
            => new User()
            {
                Id = Guid.NewGuid().ToString(),
                Email = loginRegisterDto.Email,
                FirstName = loginRegisterDto.FirstName,
                LastName = loginRegisterDto.LastName
            };

        public static UserDto MapToUserDto(this User user)
            => new UserDto()
            {
                Id = user.Id,
                FullName = $"{user.FirstName} {user.LastName}"
            };
    }
}
