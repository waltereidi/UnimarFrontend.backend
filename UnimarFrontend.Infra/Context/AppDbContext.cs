using Microsoft.EntityFrameworkCore;
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

            b.Entity<User>().ToTable("Users");
            b.Entity<User>().HasKey(u => u.Id);

            // Email VO
            b.Entity<User>().OwnsOne(u => u.Email, email =>
            {
                email.Property(e => e.Value)
                    .HasColumnName("Email")
                    .HasMaxLength(200)
                    .IsRequired();
            });

            // Password VO
            b.Entity<User>().OwnsOne(u => u.PasswordHash, password =>
            {
                password.Property(p => p.Hash)
                    .HasColumnName("PasswordHash")
                    .HasMaxLength(256)
                    .IsRequired();
            });

            b.Entity<User>().Property(u => u.CreatedAt)
                    .IsRequired();
        }
    }
}
