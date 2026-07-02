using BilsoftAnketPlatformu.Models;
using Microsoft.AspNetCore.Identity;
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
        public DbSet<Anket> Anketler { get; set; }
        public DbSet<AnketSoru> AnketSorular { get; set; }
        public DbSet<AnketCevap> AnketCevaplar { get; set; }
        public DbSet<AnketMusteri> AnketMusterileri { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Personel>(entity =>
            {
                entity.ToTable("Personel");
                entity.HasKey(e => e.PersonelID);
                entity.Property(e => e.PersonelID).HasColumnName("PersonelID");
                entity.Property(e => e.AdSoyad).HasMaxLength(150).IsRequired();
                entity.Property(e => e.AktifMi).IsRequired();

                entity.HasData(
                    new Personel { PersonelID = 1, AdSoyad = "Ahmet Yilmaz", AktifMi = true },
                    new Personel { PersonelID = 2, AdSoyad = "Ayse Demir", AktifMi = true },
                    new Personel { PersonelID = 3, AdSoyad = "Mehmet Kaya", AktifMi = true }
                );
            });

            builder.Entity<AppUserPersonel>(entity =>
            {
                entity.ToTable("AppUserPersonel");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.PersonelID).IsRequired();
                entity.HasIndex(e => new { e.UserId, e.PersonelID }).IsUnique();

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Personel)
                    .WithMany(e => e.AppUserPersoneller)
                    .HasForeignKey(e => e.PersonelID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasData(new AppUserPersonel
                {
                    Id = 1,
                    UserId = "22222222-2222-2222-2222-222222222222",
                    PersonelID = 2
                });
            });

            builder.Entity<AppRole>().HasData(
                new AppRole
                {
                    Id = "admin-role-id",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "admin-role-stamp"
                },
                new AppRole
                {
                    Id = "kullanici-role-id",
                    Name = "Kullanici",
                    NormalizedName = "KULLANICI",
                    ConcurrencyStamp = "kullanici-role-stamp"
                }
            );

            builder.Entity<AppUser>().HasData(
                new AppUser
                {
                    Id = "11111111-1111-1111-1111-111111111111",
                    UserName = "admin@bilsoft.com",
                    NormalizedUserName = "ADMIN@BILSOFT.COM",
                    Email = "admin@bilsoft.com",
                    NormalizedEmail = "ADMIN@BILSOFT.COM",
                    EmailConfirmed = true,
                    LockoutEnabled = true,
                    PasswordHash = "AQAAAAIAAYagAAAAEMygiBagleiMV23HKs8YExyJ/zKlwTYtbsWe+fn1f2psIg5fBcuXIBFepu+XT3Qq0Q==",
                    SecurityStamp = "admin-security-stamp",
                    ConcurrencyStamp = "admin-concurrency-stamp",
                    AdSoyad = "Sistem Admin",
                    AktifMi = true
                },
                new AppUser
                {
                    Id = "22222222-2222-2222-2222-222222222222",
                    UserName = "ayse@bilsoft.com",
                    NormalizedUserName = "AYSE@BILSOFT.COM",
                    Email = "ayse@bilsoft.com",
                    NormalizedEmail = "AYSE@BILSOFT.COM",
                    EmailConfirmed = true,
                    LockoutEnabled = true,
                    PasswordHash = "AQAAAAIAAYagAAAAEDaj7mispSCG7o7/wiv8A2QH+/tXM44u4hEAqnCQl4r1zyYUNnVtqw2vUW3/cDWALQ==",
                    SecurityStamp = "ayse-security-stamp",
                    ConcurrencyStamp = "ayse-concurrency-stamp",
                    AdSoyad = "Ayse Kullanici",
                    AktifMi = true
                }
            );

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = "11111111-1111-1111-1111-111111111111",
                    RoleId = "admin-role-id"
                },
                new IdentityUserRole<string>
                {
                    UserId = "22222222-2222-2222-2222-222222222222",
                    RoleId = "kullanici-role-id"
                }
            );

            builder.Entity<Anket>(entity =>
            {
                entity.ToTable("anket", table => table.ExcludeFromMigrations());
                entity.HasKey(e => e.AnketID);
                entity.Property(e => e.AnketID).HasColumnName("anketID");
                entity.Property(e => e.AnketAdi).HasColumnName("anket_adi").HasMaxLength(900);
                entity.Property(e => e.Aciklama).HasColumnName("aciklama").HasMaxLength(100);
            });

            builder.Entity<AnketSoru>(entity =>
            {
                entity.ToTable("anket_sorular", table => table.ExcludeFromMigrations());
                entity.HasKey(e => e.SoruID);
                entity.Property(e => e.SoruID).HasColumnName("soruID");
                entity.Property(e => e.SoruTipi).HasColumnName("soruTipi");
                entity.Property(e => e.Soru).HasColumnName("soru").HasMaxLength(200);
                entity.Property(e => e.AnketID).HasColumnName("anketID");
                entity.Property(e => e.SiraNo).HasColumnName("siraNo");
            });

            builder.Entity<AnketCevap>(entity =>
            {
                entity.ToTable("anket_cevaplar", table => table.ExcludeFromMigrations());
                entity.HasKey(e => e.CevapID);
                entity.Property(e => e.CevapID).HasColumnName("cevapID");
                entity.Property(e => e.ServisNo).HasColumnName("servisNo");
                entity.Property(e => e.PersonelID).HasColumnName("personelID");
                entity.Property(e => e.AnketID).HasColumnName("anketID");
                entity.Property(e => e.SoruID).HasColumnName("soruID");
                entity.Property(e => e.SoruTipi).HasColumnName("soruTipi");
                entity.Property(e => e.Cevap).HasColumnName("cevap");
                entity.Property(e => e.KayitTarih).HasColumnName("kayitTarih");
            });

            builder.Entity<AnketMusteri>(entity =>
            {
                entity.ToTable("anket_musteri", table => table.ExcludeFromMigrations());
                entity.HasNoKey();
                entity.Property(e => e.ServisID).HasColumnName("servisID");
                entity.Property(e => e.PersonelID).HasColumnName("personelID");
                entity.Property(e => e.AnketID).HasColumnName("anketID");
                entity.Property(e => e.AnketDurumu).HasColumnName("anketDurumu");
                entity.Property(e => e.AnketTarihKayit).HasColumnName("anketTarihKayit");
            });
        }
    }
}
