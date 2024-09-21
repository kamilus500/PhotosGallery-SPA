using Microsoft.AspNetCore.Http;
using PhotosGallerySPA.Domain.Entities;
using PhotosGallerySPA.Infrastructure.Extensions;
using PhotosGallerySPA.Infrastructure.Services.Interfaces;

namespace PhotosGallerySPA.Infrastructure.Middlewares
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context, IErrorService errorService)
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
                    CreationDate = DateTimeProvider.DateNowUtc,
                    Exception = ex.Message.ToString()
                };

                await errorService.Create(errorTable);
            }
            
        }
    }
}
