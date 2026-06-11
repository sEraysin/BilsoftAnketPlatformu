using BilsoftAnketPlatformu.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BilsoftAnketPlatformu.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Personel> Personeller { get; set; }
        public DbSet<AppUserPersonel> AppUserPersoneller { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Personel>(entity =>
            {
                entity.ToTable("Personel");

                entity.HasKey(e => e.PersonelID);

                entity.Property(e => e.PersonelID)
                    .HasColumnName("PersonelID");

                entity.Property(e => e.AdSoyad)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(e => e.AktifMi)
                    .IsRequired();
            });

            builder.Entity<AppUserPersonel>(entity =>
            {
                entity.ToTable("AppUserPersonel");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.UserId)
                    .IsRequired();

                entity.Property(e => e.PersonelID)
                    .IsRequired();

                entity.HasIndex(e => new { e.UserId, e.PersonelID })
                    .IsUnique();

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Personel)
                    .WithMany(e => e.AppUserPersoneller)
                    .HasForeignKey(e => e.PersonelID)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}