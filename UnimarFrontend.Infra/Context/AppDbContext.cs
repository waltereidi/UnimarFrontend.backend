using Microsoft.EntityFrameworkCore;
using UnimarFrontend.backend.UnimarFrontend.Dominio.Entidades;
using UnimarFrontend.backend.ValueObjects;
using UnimarFrontend.Dominio.Entidades;
namespace UnimarFrontend.backend.UnimarFrontend.Infra.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookComment> BookComments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BookGoogleDrive> BookGoogleDrive { get; set; }
        public DbSet<BookFileStorage> BookFileStorages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureUser(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void NormalizeDateTimes()
        {
            var entries = ChangeTracker.Entries<Entity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }
        }
        public override int SaveChanges()
        {
            NormalizeDateTimes();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            NormalizeDateTimes();
            return base.SaveChangesAsync(cancellationToken);
        }
        public void ConfigureUser(ModelBuilder b)
        {

            b.Entity<User>().ToTable("Users");
            b.Entity<User>().HasKey(u => u.Id);

            b.Entity<User>().Property(u => u.PasswordHash)
                .HasConversion(
                    v => v.Hash,
                    v => new PasswordVO(v)
                );

            b.Entity<User>().Property(u => u.Email)
                .HasConversion(
                    v => v.Value,
                    v => new EmailVO(v)
                );

            b.Entity<User>().Property(u => u.CreatedAt)
                    .IsRequired();
        }
    }
}
