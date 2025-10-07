using System;
using System.ComponentModel.DataAnnotations;

namespace FabrikaAPI.Models
{
    public class UrunTipi
    {
        [Key]
        public int UrunTipiID { get; set; }

        [Required]
        [StringLength(100)]
        public string UrunAdi { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string UrunKodu { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Aciklama { get; set; }

        [Required]
        public double MinKaliteSkoru { get; set; } = 70.0; // Minimum kabul edilebilir kalite skoru

        [Required]
        public double MaxKaliteSkoru { get; set; } = 100.0; // Maksimum kalite skoru

        [Required]
        public bool AktifMi { get; set; } = true;

        // İlişkili üretilen ürünler
        public virtual ICollection<Urun>? Urunler { get; set; }
    }
}