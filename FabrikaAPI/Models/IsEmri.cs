using System;
using System.ComponentModel.DataAnnotations;

namespace FabrikaAPI.Models
{
    public class IsEmri
    {
        [Key]
        public int IsEmriID { get; set; }

        [Required]
        [StringLength(50)]
        public string IsEmriKodu { get; set; } = string.Empty;

        [Required]
        public DateTime BaslangicTarihi { get; set; }

        public DateTime? BitisTarihi { get; set; }

        [Required]
        [StringLength(20)]
        public string Durum { get; set; } = "Beklemede"; // Beklemede, Devam Ediyor, Tamamlandı

        [StringLength(500)]
        public string? Aciklama { get; set; }

        // İş emrine bağlı ürün
        public int? UrunID { get; set; }
        public virtual Urun? Urun { get; set; }
    }
}