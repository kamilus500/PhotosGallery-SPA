namespace PhotosGallerySPA.Domain.Entities
{
    public class Photo
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string FileName { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
