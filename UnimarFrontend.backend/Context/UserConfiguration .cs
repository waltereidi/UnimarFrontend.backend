using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnimarFrontend.backend.Models;

namespace UnimarFrontend.backend.Context
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

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
