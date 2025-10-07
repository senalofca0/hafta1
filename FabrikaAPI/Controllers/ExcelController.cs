using Microsoft.AspNetCore.Mvc;
using FabrikaAPI.Models;
using FabrikaAPI.Data;
using OfficeOpenXml;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace FabrikaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelController : ControllerBase
    {
        private readonly FabrikaContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ExcelController(FabrikaContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            // EPPlus lisans ayarı
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        [HttpPost("import")]
        public async Task<IActionResult> Import([FromForm] ExcelImport request)
        {
            try
            {
                if (request.File == null || request.File.Length == 0)
                    return BadRequest("Dosya yüklenemedi.");

                string fileExtension = Path.GetExtension(request.File.FileName);
                if (fileExtension != ".xlsx")
                    return BadRequest("Sadece .xlsx uzantılı dosyalar desteklenmektedir.");

                using var stream = new MemoryStream();
                await request.File.CopyToAsync(stream);
                using var package = new ExcelPackage(stream);
                
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension?.Rows ?? 0;

                switch (request.ImportType?.ToLower())
                {
                    case "urunler":
                        return await ImportUrunler(worksheet, rowCount);
                    case "uruntipleri":
                        return await ImportUrunTipleri(worksheet, rowCount);
                    case "isemirleri":
                        return await ImportIsEmirleri(worksheet, rowCount);
                    default:
                        return BadRequest("Geçersiz import türü.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"İçe aktarma sırasında bir hata oluştu: {ex.Message}");
            }
        }

        [HttpGet("export")]
        public async Task<IActionResult> Export([FromQuery] ExcelExportRequest request)
        {
            try
            {
                var stream = new MemoryStream();
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.Add("Veriler");
                    
                    switch (request.ExportType.ToLower())
                    {
                        case "urunler":
                            await ExportUrunler(worksheet);
                            break;
                        case "uruntipleri":
                            await ExportUrunTipleri(worksheet);
                            break;
                        case "isemirleri":
                            await ExportIsEmirleri(worksheet, request.StartDate, request.EndDate);
                            break;
                        default:
                            return BadRequest("Geçersiz export türü.");
                    }

                    await package.SaveAsync();
                }

                stream.Position = 0;
                string fileName = $"{request.ExportType}_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Dışa aktarma sırasında bir hata oluştu: {ex.Message}");
            }
        }

        private async Task<IActionResult> ImportUrunler(ExcelWorksheet worksheet, int rowCount)
        {
            var urunler = new List<Urun>();
            
            for (int row = 2; row <= rowCount; row++)
            {
                var urun = new Urun
                {
                    SeriNo = worksheet.Cells[row, 1].Value?.ToString() ?? "",
                    UretimTarihi = worksheet.Cells[row, 2].Value != null ? 
                        DateTime.FromOADate(double.Parse(worksheet.Cells[row, 2].Value.ToString())) :
                        DateTime.Now,
                    KaliteSkoru = double.Parse(worksheet.Cells[row, 3].Value?.ToString() ?? "0"),
                    Durum = worksheet.Cells[row, 4].Value?.ToString() ?? "Aktif",
                    UrunTipiID = int.Parse(worksheet.Cells[row, 5].Value?.ToString() ?? "0")
                };
                
                urunler.Add(urun);
            }

            await _context.Urunler.AddRangeAsync(urunler);
            await _context.SaveChangesAsync();
            
            return Ok($"{urunler.Count} ürün başarıyla içe aktarıldı.");
        }

        private async Task<IActionResult> ImportUrunTipleri(ExcelWorksheet worksheet, int rowCount)
        {
            var urunTipleri = new List<UrunTipi>();
            
            for (int row = 2; row <= rowCount; row++)
            {
                var urunTipi = new UrunTipi
                {
                    UrunAdi = worksheet.Cells[row, 1].Value?.ToString() ?? "",
                    UrunKodu = worksheet.Cells[row, 2].Value?.ToString() ?? "",
                    Aciklama = worksheet.Cells[row, 3].Value?.ToString(),
                    MinKaliteSkoru = double.Parse(worksheet.Cells[row, 4].Value?.ToString() ?? "0"),
                    MaxKaliteSkoru = double.Parse(worksheet.Cells[row, 5].Value?.ToString() ?? "100"),
                    AktifMi = bool.Parse(worksheet.Cells[row, 6].Value?.ToString() ?? "true")
                };
                
                urunTipleri.Add(urunTipi);
            }

            await _context.UrunTipleri.AddRangeAsync(urunTipleri);
            await _context.SaveChangesAsync();
            
            return Ok($"{urunTipleri.Count} ürün tipi başarıyla içe aktarıldı.");
        }

        private async Task<IActionResult> ImportIsEmirleri(ExcelWorksheet worksheet, int rowCount)
        {
            var isEmirleri = new List<IsEmri>();
            
            for (int row = 2; row <= rowCount; row++)
            {
                var isEmri = new IsEmri
                {
                    IsEmriKodu = worksheet.Cells[row, 1].Value?.ToString() ?? "",
                    UrunID = int.Parse(worksheet.Cells[row, 2].Value?.ToString() ?? "0"),
                    BaslangicTarihi = worksheet.Cells[row, 3].Value != null ? 
                        DateTime.TryParse(worksheet.Cells[row, 3].Value.ToString(), out var baslangicTarihi) ? 
                            baslangicTarihi : DateTime.Now :
                        DateTime.Now,
                    BitisTarihi = worksheet.Cells[row, 4].Value != null ? 
                        DateTime.TryParse(worksheet.Cells[row, 4].Value.ToString(), out var bitisTarihi) ? 
                            bitisTarihi : DateTime.Now.AddDays(1) :
                        DateTime.Now.AddDays(1),
                    Durum = worksheet.Cells[row, 5].Value?.ToString() ?? "Beklemede",
                    Aciklama = worksheet.Cells[row, 6].Value?.ToString()
                };
                
                isEmirleri.Add(isEmri);
            }

            await _context.IsEmirleri.AddRangeAsync(isEmirleri);
            await _context.SaveChangesAsync();
            
            return Ok($"{isEmirleri.Count} iş emri başarıyla içe aktarıldı.");
        }

        private async Task ExportUrunler(ExcelWorksheet worksheet)
        {
            var urunler = await _context.Urunler.ToListAsync();
            
            // Başlıkları ayarla
            worksheet.Cells[1, 1].Value = "Seri No";
            worksheet.Cells[1, 2].Value = "Üretim Tarihi";
            worksheet.Cells[1, 3].Value = "Kalite Puanı";
            worksheet.Cells[1, 4].Value = "Durum";
            worksheet.Cells[1, 5].Value = "Ürün Tipi ID";

            // Verileri doldur
            for (int i = 0; i < urunler.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = urunler[i].SeriNo;
                worksheet.Cells[i + 2, 2].Value = urunler[i].UretimTarihi;
                worksheet.Cells[i + 2, 3].Value = urunler[i].KaliteSkoru;
                worksheet.Cells[i + 2, 4].Value = urunler[i].Durum;
                worksheet.Cells[i + 2, 5].Value = urunler[i].UrunTipiID;
            }

            // Stil ayarları
            using (var range = worksheet.Cells[1, 1, 1, 5])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            }

            worksheet.Cells.AutoFitColumns();
        }

        private async Task ExportUrunTipleri(ExcelWorksheet worksheet)
        {
            var urunTipleri = await _context.UrunTipleri.ToListAsync();
            
            // Başlıkları ayarla
            worksheet.Cells[1, 1].Value = "Ürün Adı";
            worksheet.Cells[1, 2].Value = "Ürün Kodu";
            worksheet.Cells[1, 3].Value = "Açıklama";
            worksheet.Cells[1, 4].Value = "Min Kalite Skoru";
            worksheet.Cells[1, 5].Value = "Max Kalite Skoru";
            worksheet.Cells[1, 6].Value = "Aktif Mi";

            // Verileri doldur
            for (int i = 0; i < urunTipleri.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = urunTipleri[i].UrunAdi;
                worksheet.Cells[i + 2, 2].Value = urunTipleri[i].UrunKodu;
                worksheet.Cells[i + 2, 3].Value = urunTipleri[i].Aciklama;
                worksheet.Cells[i + 2, 4].Value = urunTipleri[i].MinKaliteSkoru;
                worksheet.Cells[i + 2, 5].Value = urunTipleri[i].MaxKaliteSkoru;
                worksheet.Cells[i + 2, 6].Value = urunTipleri[i].AktifMi;
            }

            // Stil ayarları
            using (var range = worksheet.Cells[1, 1, 1, 6])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            }

            worksheet.Cells.AutoFitColumns();
        }

        private async Task ExportIsEmirleri(ExcelWorksheet worksheet, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.IsEmirleri.AsQueryable();
            
            if (startDate.HasValue)
                query = query.Where(i => i.BaslangicTarihi >= startDate.Value);
            
            if (endDate.HasValue)
                query = query.Where(i => i.BitisTarihi <= endDate.Value);

            var isEmirleri = await query.ToListAsync();
            
            // Başlıkları ayarla
            worksheet.Cells[1, 1].Value = "İş Emri Kodu";
            worksheet.Cells[1, 2].Value = "Ürün ID";
            worksheet.Cells[1, 3].Value = "Başlangıç Tarihi";
            worksheet.Cells[1, 4].Value = "Bitiş Tarihi";
            worksheet.Cells[1, 5].Value = "Durum";
            worksheet.Cells[1, 6].Value = "Açıklama";

            // Verileri doldur
            for (int i = 0; i < isEmirleri.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = isEmirleri[i].IsEmriKodu;
                worksheet.Cells[i + 2, 2].Value = isEmirleri[i].UrunID;
                worksheet.Cells[i + 2, 3].Value = isEmirleri[i].BaslangicTarihi;
                worksheet.Cells[i + 2, 4].Value = isEmirleri[i].BitisTarihi;
                worksheet.Cells[i + 2, 5].Value = isEmirleri[i].Durum;
                worksheet.Cells[i + 2, 6].Value = isEmirleri[i].Aciklama;
            }

            // Stil ayarları
            using (var range = worksheet.Cells[1, 1, 1, 6])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            }

            worksheet.Cells.AutoFitColumns();
        }
    }
}