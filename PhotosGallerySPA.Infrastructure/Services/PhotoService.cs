using Microsoft.EntityFrameworkCore;
using PhotosGallerySPA.Domain.Dtos.Photo;
using PhotosGallerySPA.Domain.Entities;
using PhotosGallerySPA.Infrastructure.Extensions;
using PhotosGallerySPA.Infrastructure.Persistance;
using PhotosGallerySPA.Infrastructure.Services.Interfaces;

namespace PhotosGallerySPA.Infrastructure.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ISessionService _sessionService;
        public PhotoService(ApplicationDbContext dbContext, ISessionService sessionService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
        }

        public async Task CreatePhoto(CreatePhotoDto photo)
        {
            var newPhoto = new Photo()
            {
                CreationDate = DateTime.UtcNow,
                Description = photo.Description,
                Title = photo.Title,
                UserId = photo.UserId,
                Id = Guid.NewGuid().ToString()
            };

            if (newPhoto is null)
                throw new ArgumentNullException(nameof(newPhoto));

            newPhoto.Image = await photo.Image.ToByteArrayAsync();

            await _dbContext.Photos.AddAsync(newPhoto);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeletePhoto(string id)
        {
            var photo = await _dbContext.Photos.FirstOrDefaultAsync(x => x.Id == id);

            if (photo is null)
                throw new ArgumentNullException(nameof(photo));

            photo.IsDeleted = true;

            await _dbContext.SaveChangesAsync();
        }

        public async Task<PhotoDto> GetPhoto(string id)
        {
            var photo = await _dbContext.Photos.FirstOrDefaultAsync(x => x.Id == id);

            return photo.MapToPhotoDto();
        }

        public async Task<IEnumerable<PhotoDto>> GetPhotos()
        {
            var userId = _sessionService.GetValue("UserId");

            var photos = await _dbContext.Photos
                                            .Where(x => x.UserId == userId && !x.IsDeleted)
                                            .AsNoTracking()
                                            .ToListAsync();

            var photosDto = new List<PhotoDto>();

            foreach (var photo in photos)
            {
                photosDto.Add(photo.MapToPhotoDto());
            }

            return photosDto;
        }
    }
}