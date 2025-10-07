using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FabrikaAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Urunler",
                columns: table => new
                {
                    UrunID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeriNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UretimTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KaliteSkoru = table.Column<double>(type: "float", nullable: false),
                    Durum = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Beklemede")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Urunler", x => x.UrunID);
                });

            migrationBuilder.InsertData(
                table: "Urunler",
                columns: new[] { "UrunID", "Durum", "KaliteSkoru", "SeriNo", "UretimTarihi" },
                values: new object[,]
                {
                    { 1, "Tamamlandı", 95.5, "ABC123", new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Üretimde", 88.700000000000003, "XYZ789", new DateTime(2024, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Kalite Kontrolde", 92.299999999999997, "DEF456", new DateTime(2024, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Tamamlandı", 97.099999999999994, "GHI789", new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "Beklemede", 85.900000000000006, "JKL012", new DateTime(2024, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Urunler");
        }
    }
}
