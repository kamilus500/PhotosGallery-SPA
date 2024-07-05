namespace PhotosGallerySPA.Domain.Dtos.Photo
{
    public class PhotoDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public byte[] Image { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
    }
}
