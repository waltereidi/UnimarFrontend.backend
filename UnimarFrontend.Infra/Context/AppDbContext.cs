using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnimarFrontend.backend.UnimarFrontend.Dominio.Entidades;

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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureUser(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        public void ConfigureUser(ModelBuilder b)
        {
            var builder = b.Entity<User>();
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);

            // Email VO
            builder.OwnsOne(u => u.Email, email =>
            {
                email.Property(e => e.Value)
                    .HasColumnName("Email")
                    .HasMaxLength(200)
                    .IsRequired();
            });

            // Password VO
            builder.OwnsOne(u => u.PasswordHash, password =>
            {
                password.Property(p => p.Hash)
                    .HasColumnName("PasswordHash")
                    .HasMaxLength(256)
                    .IsRequired();
            });

            builder.Property(u => u.CreatedAt)
                    .IsRequired();
        }
    }
}
