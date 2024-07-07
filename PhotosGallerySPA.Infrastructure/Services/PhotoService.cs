using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _memoryCache;
        private readonly ISessionService _sessionService;
        public PhotoService(ApplicationDbContext dbContext, ISessionService sessionService, IMemoryCache memoryCache)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public async Task<bool> CreatePhoto(CreatePhotoDto photo)
        {
            try
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

                _memoryCache.Remove("getphotos");

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeletePhoto(string id)
        {
            try
            {
                var photo = await _dbContext.Photos.FirstOrDefaultAsync(x => x.Id == id);

                if (photo is null)
                    throw new ArgumentNullException(nameof(photo));

                photo.IsDeleted = true;

                await _dbContext.SaveChangesAsync();

                _memoryCache.Remove("getphotos");

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<PhotoDto> GetPhoto(string id)
        {
            var cacheKey = $"getphoto-{id}";
            if (!_memoryCache.TryGetValue(cacheKey, out PhotoDto photoDto))
            {

                var photo = await _dbContext.Photos.FirstOrDefaultAsync(x => x.Id == id);

                photoDto = photo.MapToPhotoDto();

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                };

                _memoryCache.Set(cacheKey, photoDto, cacheEntryOptions);
            }

            return photoDto;
        }

        public async Task<IEnumerable<PhotoDto>> GetPhotos()
        {
            var userId = _sessionService.GetValue("UserId");

            var cacheKey = $"getphotos";
            if (!_memoryCache.TryGetValue(cacheKey, out List<PhotoDto> photosDto))
            {
                var photos = await _dbContext.Photos
                                .Where(x => x.UserId == userId && !x.IsDeleted)
                                .Include(x => x.User)
                                .AsNoTracking()
                                .ToListAsync();

                photosDto = photos.MapToPhotoDtoList();

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                };

                _memoryCache.Set(cacheKey, photosDto, cacheEntryOptions);
            }

            return photosDto;
        }
    }
}