using Microsoft.EntityFrameworkCore;
using UnimarFrontend.backend.Models;

namespace UnimarFrontend.backend.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }

    }
}
