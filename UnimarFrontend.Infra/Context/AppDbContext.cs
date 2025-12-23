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
        public DbSet<FileStorage> FileStorage { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureUser(modelBuilder);
            ConfigureBook(modelBuilder);
            ConfigureBookGoogleDrive(modelBuilder);
            ConfigureBookFileStorage(modelBuilder);
            ConfigureBookComment(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
        private void ConfigureBookFileStorage(ModelBuilder b)
        {
            b.Entity<BookFileStorage>()
                .ToTable("BookFileStorages");

            b.Entity<BookFileStorage>()
                .HasKey(fs => fs.Id);

            b.Entity<BookFileStorage>()
                .Property(fs => fs.BookId)
                .IsRequired();

            b.Entity<BookFileStorage>()
                .Property(fs => fs.FileStorageId)
                .IsRequired();

            // Book 1:N BookFileStorage
            b.Entity<BookFileStorage>()
                .HasOne(fs => fs.Book)
                .WithMany(bk => bk.BookFileStorages)
                .HasForeignKey(fs => fs.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            // FileStorage 1:N BookFileStorage
            b.Entity<BookFileStorage>()
                .HasOne(fs => fs.FileStorage)
                .WithMany(f => f.BookFileStorages)
                .HasForeignKey(fs => fs.FileStorageId)
                .OnDelete(DeleteBehavior.Cascade);
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
        private void ConfigureBookGoogleDrive(ModelBuilder b)
        {
            b.Entity<BookGoogleDrive>()
                .ToTable("BookGoogleDrive");

            b.Entity<BookGoogleDrive>()
                .HasKey(gd => gd.Id);

            b.Entity<BookGoogleDrive>()
                .Property(gd => gd.BookId)
                .IsRequired();
        }
        public override Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            NormalizeDateTimes();
            return base.SaveChangesAsync(cancellationToken);
        }
        private void ConfigureBook(ModelBuilder b)
        {
            b.Entity<Book>()
                .ToTable("Books");

            b.Entity<Book>()
                .HasKey(bk => bk.Id);

            b.Entity<Book>()
                .Property(bk => bk.CreatedAt)
                .IsRequired();

            // Book 1:N BookGoogleDrive
            b.Entity<Book>()
                .HasMany(bk => bk.BookGoogleDrives)
                .WithOne(gd => gd.Book)
                .HasForeignKey(gd => gd.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            // Book 1:N BookFileStorage
            b.Entity<Book>()
                .HasMany(bk => bk.BookFileStorages)
                .WithOne(fs => fs.Book)
                .HasForeignKey(fs => fs.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            // Book 1:N BookComment
            b.Entity<Book>()
                .HasMany(bk => bk.BookComments)
                .WithOne(c => c.Book)
                .HasForeignKey(c => c.BookId)
                .OnDelete(DeleteBehavior.Cascade);
        }
        private void ConfigureBookComment(ModelBuilder b)
        {
            b.Entity<BookComment>()
                .ToTable("BookComments");

            b.Entity<BookComment>()
                .HasKey(c => c.Id);

            b.Entity<BookComment>()
                .Property(c => c.BookId)
                .IsRequired();

            b.Entity<BookComment>()
                .Property(c => c.UserId)
                .IsRequired();

            // Comment N:1 Book
            b.Entity<BookComment>()
                .HasOne(c => c.Book)
                .WithMany(bk => bk.BookComments)
                .HasForeignKey(c => c.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            // Comment N:1 User
            b.Entity<BookComment>()
                .HasOne(c => c.User)
                .WithMany(u => u.BookComments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);
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

            b.Entity<User>()
                .HasMany(u => u.BookComments)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId);

        }
    }
}
