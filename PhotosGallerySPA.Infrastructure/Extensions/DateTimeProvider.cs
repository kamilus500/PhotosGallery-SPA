namespace PhotosGallerySPA.Infrastructure.Extensions
{
    public struct DateTimeProvider
    {
        public static DateTime DateNowUtc
        {
            get
            {
                return DateTime.UtcNow;
            }
        }
    }
}
