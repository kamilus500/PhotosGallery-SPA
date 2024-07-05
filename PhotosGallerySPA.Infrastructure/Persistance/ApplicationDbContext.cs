using Microsoft.EntityFrameworkCore;
using PhotosGallerySPA.Domain.Entities;

namespace PhotosGallerySPA.Infrastructure.Persistance
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<ErrorTable> Errors { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
