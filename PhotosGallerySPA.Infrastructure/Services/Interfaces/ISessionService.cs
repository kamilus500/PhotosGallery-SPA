namespace PhotosGallerySPA.Infrastructure.Services.Interfaces
{
    public interface ISessionService
    {
        public string GetValue(string key);
        public void SetValue(string key, string value);
        public void Clear();
    }
}
