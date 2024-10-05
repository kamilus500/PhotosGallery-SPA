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
        private readonly IErrorService _errorService;
        public PhotoService(ApplicationDbContext dbContext, ISessionService sessionService, IMemoryCache memoryCache, IErrorService errorService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _errorService = errorService ?? throw new ArgumentException(nameof(errorService));
        }

        public async Task<(bool, string)> CreatePhoto(CreatePhotoDto photo, string rootPath)
        {
            try
            {
                var fileName = $"{photo.Image.FileName}_{DateTime.UtcNow}";

                fileName = fileName.Replace(".jpg", "");

                fileName = fileName.Replace(' ', '_');

                fileName = fileName.Replace(':', '_');

                fileName += ".jpg";

                var path = Path.Combine($"{rootPath}\\images", $"{ fileName }");

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await photo.Image.CopyToAsync(stream);
                }

                var newPhoto = new Photo()
                {
                    CreationDate = DateTimeProvider.DateNowUtc,
                    Description = photo.Description,
                    Title = photo.Title,
                    FileName = path,
                    UserId = photo.UserId,
                    Id = Guid.NewGuid().ToString()
                };

                await _dbContext.Photos.AddAsync(newPhoto);
                await _dbContext.SaveChangesAsync();

                _memoryCache.Remove("getphotos");

                return (true, string.Empty);
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

        public async Task<(bool, string)> DeletePhoto(string id, string rootPath)
        {
            try
            {
                var photo = await _dbContext.Photos.FirstOrDefaultAsync(x => x.Id == id);

                if (photo is null)
                    return (false, "Photo doesnt exist");

                photo.IsDeleted = true;

                await _dbContext.SaveChangesAsync();

                var path = Path.Combine($"{rootPath}\\images", $"{photo.FileName}");

                File.Delete(path);

                _memoryCache.Remove("getphotos");

                return (true, string.Empty);
            }
            catch (Exception ex)
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