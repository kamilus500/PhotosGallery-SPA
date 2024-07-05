using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using PhotosGallerySPA.Infrastructure.Hubs;
using PhotosGallerySPA.Infrastructure.Services.Interfaces;

namespace PhotosGallerySPA.Infrastructure.CustomAttributes
{
    public class AuthorizationFilter : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var sessionService = context.HttpContext.RequestServices.GetRequiredService<ISessionService>();
            var hubContext = context.HttpContext.RequestServices.GetRequiredService<IHubContext<AccessHub>>();

            var userId = sessionService.GetValue("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                await hubContext.Clients.All.SendAsync("AccessDeniedNotification");
                return;
            }
        }
    }
}
