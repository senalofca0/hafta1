using Microsoft.EntityFrameworkCore;
using FabrikaAPI.Models;

namespace FabrikaAPI.Data
{
    /// <summary>
    /// Veritabanı bağlantısını ve tablolarını yöneten context sınıfı
    /// </summary>
    public class FabrikaContext : DbContext
    {
        public FabrikaContext(DbContextOptions<FabrikaContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Ürünler tablosu
        /// </summary>
        public DbSet<Urun> Urunler { get; set; } = null!;

        /// <summary>
        /// İş emirleri tablosu
        /// </summary>
        public DbSet<IsEmri> IsEmirleri { get; set; } = null!;

        /// <summary>
        /// Ürün tipleri tablosu
        /// </summary>
        public DbSet<UrunTipi> UrunTipleri { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ürün entity'si için konfigürasyonlar
            modelBuilder.Entity<Urun>(entity =>
            {
                entity.HasKey(e => e.UrunID);
                entity.Property(e => e.SeriNo).IsRequired().HasMaxLength(50);
                entity.Property(e => e.UretimTarihi).IsRequired();
                entity.Property(e => e.KaliteSkoru).IsRequired();
                entity.Property(e => e.Durum).IsRequired().HasDefaultValue("Beklemede");

                // UrunTipi ile ilişki
                entity.HasOne(u => u.UrunTipi)
                    .WithMany(t => t.Urunler)
                    .HasForeignKey(u => u.UrunTipiID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Örnek ürün tipi verilerinin eklenmesi
            modelBuilder.Entity<UrunTipi>().HasData(
                new UrunTipi
                {
                    UrunTipiID = 1,
                    UrunAdi = "Standart Ürün A",
                    UrunKodu = "STD-A",
                    Aciklama = "Standart A tipi ürün",
                    MinKaliteSkoru = 80.0,
                    MaxKaliteSkoru = 100.0,
                    AktifMi = true
                },
                new UrunTipi
                {
                    UrunTipiID = 2,
                    UrunAdi = "Premium Ürün B",
                    UrunKodu = "PRE-B",
                    Aciklama = "Premium B tipi ürün",
                    MinKaliteSkoru = 90.0,
                    MaxKaliteSkoru = 100.0,
                    AktifMi = true
                },
                new UrunTipi
                {
                    UrunTipiID = 3,
                    UrunAdi = "Ekonomik Ürün C",
                    UrunKodu = "ECO-C",
                    Aciklama = "Ekonomik C tipi ürün",
                    MinKaliteSkoru = 70.0,
                    MaxKaliteSkoru = 90.0,
                    AktifMi = true
                }
            );

            // Örnek ürün verilerinin eklenmesi
            modelBuilder.Entity<Urun>().HasData(
                new Urun
                {
                    UrunID = 1,
                    SeriNo = "ABC123",
                    UretimTarihi = new DateTime(2024, 3, 1),
                    KaliteSkoru = 95.5,
                    Durum = "Tamamlandı",
                    UrunTipiID = 2 // Premium Ürün
                },
                new Urun
                {
                    UrunID = 2,
                    SeriNo = "XYZ789",
                    UretimTarihi = new DateTime(2024, 3, 3),
                    KaliteSkoru = 88.7,
                    Durum = "Üretimde",
                    UrunTipiID = 1 // Standart Ürün
                },
                new Urun
                {
                    UrunID = 3,
                    SeriNo = "DEF456",
                    UretimTarihi = new DateTime(2024, 3, 4),
                    KaliteSkoru = 92.3,
                    Durum = "Kalite Kontrolde",
                    UrunTipiID = 2 // Premium Ürün
                },
                new Urun
                {
                    UrunID = 4,
                    SeriNo = "GHI789",
                    UretimTarihi = new DateTime(2024, 3, 5),
                    KaliteSkoru = 97.1,
                    Durum = "Tamamlandı",
                    UrunTipiID = 2 // Premium Ürün
                },
                new Urun
                {
                    UrunID = 5,
                    SeriNo = "JKL012",
                    UretimTarihi = new DateTime(2024, 3, 6),
                    KaliteSkoru = 85.9,
                    Durum = "Beklemede",
                    UrunTipiID = 1 // Standart Ürün
                }
            );

            // Örnek iş emri verilerinin eklenmesi
            modelBuilder.Entity<IsEmri>().HasData(
                new IsEmri
                {
                    IsEmriID = 1,
                    IsEmriKodu = "WO-2024-001",
                    BaslangicTarihi = new DateTime(2024, 3, 1),
                    BitisTarihi = new DateTime(2024, 3, 2),
                    Durum = "Tamamlandı",
                    Aciklama = "ABC123 seri nolu ürünün üretimi",
                    UrunID = 1
                },
                new IsEmri
                {
                    IsEmriID = 2,
                    IsEmriKodu = "WO-2024-002",
                    BaslangicTarihi = new DateTime(2024, 3, 3),
                    BitisTarihi = null,
                    Durum = "Devam Ediyor",
                    Aciklama = "XYZ789 seri nolu ürünün üretimi",
                    UrunID = 2
                },
                new IsEmri
                {
                    IsEmriID = 3,
                    IsEmriKodu = "WO-2024-003",
                    BaslangicTarihi = new DateTime(2024, 3, 4),
                    BitisTarihi = null,
                    Durum = "Beklemede",
                    Aciklama = "DEF456 seri nolu ürünün üretimi",
                    UrunID = 3
                }
            );
        }
    }
}