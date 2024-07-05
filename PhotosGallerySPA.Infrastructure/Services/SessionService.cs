using Microsoft.AspNetCore.Http;
using PhotosGallerySPA.Infrastructure.Services.Interfaces;

namespace PhotosGallerySPA.Infrastructure.Services
{
    public class SessionService : ISessionService
    {
        private readonly ISession _session;
        public SessionService(IHttpContextAccessor httpContextAccessor)
            => _session = httpContextAccessor.HttpContext.Session ?? throw new ArgumentNullException(nameof(httpContextAccessor));

        public void Clear()
            => _session.Clear();

        public string GetValue(string key)
            => _session.GetString(key);

        public void SetValue(string key, string value)
            => _session.SetString(key, value);
    }
}
