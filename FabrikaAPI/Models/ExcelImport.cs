using System.ComponentModel.DataAnnotations;

namespace FabrikaAPI.Models
{
    public class ExcelImport
    {
        [Required]
        public IFormFile File { get; set; } = null!;
        
        public string? ImportType { get; set; }
    }

    public class ExcelExportRequest
    {
        [Required]
        public string ExportType { get; set; } = string.Empty;
        
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}