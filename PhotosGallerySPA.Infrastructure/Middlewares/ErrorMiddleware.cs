using Microsoft.AspNetCore.Http;
using PhotosGallerySPA.Domain.Entities;
using PhotosGallerySPA.Infrastructure.Services.Interfaces;

namespace PhotosGallerySPA.Infrastructure.Middlewares
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IErrorService _errorService;

        public ErrorMiddleware(RequestDelegate next, IErrorService errorService)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _errorService = errorService ?? throw new ArgumentNullException(nameof(errorService));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                var errorTable = new ErrorTable()
                {
                    Id = Guid.NewGuid().ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    CreationDate = DateTime.UtcNow,
                    Exception = ex.Message.ToString()
                };

                await _errorService.Create(errorTable);
            }
            
        }
    }
}
