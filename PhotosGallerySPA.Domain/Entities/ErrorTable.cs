namespace PhotosGallerySPA.Domain.Entities
{
    public class ErrorTable
    {
        public string Id { get; set; }
        public string Exception { get; set; }
        public string StackTrace { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
