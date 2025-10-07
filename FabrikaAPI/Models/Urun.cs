using System;
using System.ComponentModel.DataAnnotations;

namespace FabrikaAPI.Models
{
    /// <summary>
    /// Ürün bilgilerini tutan model sınıfı
    /// </summary>
    public class Urun
    {
        /// <summary>
        /// Ürünün benzersiz kimlik numarası
        /// </summary>
        [Key]
        public int UrunID { get; set; }

        /// <summary>
        /// Ürünün seri numarası
        /// </summary>
        [Required]
        [StringLength(50)]
        public string SeriNo { get; set; } = string.Empty;

        /// <summary>
        /// Ürünün üretim tarihi
        /// </summary>
        [Required]
        public DateTime UretimTarihi { get; set; }

        /// <summary>
        /// Ürünün kalite skoru
        /// </summary>
        [Required]
        public double KaliteSkoru { get; set; }

        /// <summary>
        /// Ürünün durumu
        /// </summary>
        [Required]
        public string Durum { get; set; } = "Beklemede";

        /// <summary>
        /// Ürünün tipi/şablonu
        /// </summary>
        public int UrunTipiID { get; set; }
        public virtual UrunTipi? UrunTipi { get; set; }

        /// <summary>
        /// Ürüne ait iş emirleri
        /// </summary>
        public virtual ICollection<IsEmri>? IsEmirleri { get; set; }
    }
}