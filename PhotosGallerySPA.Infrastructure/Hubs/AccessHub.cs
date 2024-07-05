using Microsoft.AspNetCore.SignalR;

namespace PhotosGallerySPA.Infrastructure.Hubs
{
    public class AccessHub : Hub
    {
        public async Task Accessdenied()
        {
            await Clients.All.SendAsync("AccessDeniedNotification");
        }
    }
}
