using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FabrikaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddIsEmriTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IsEmirleri",
                columns: table => new
                {
                    IsEmriID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsEmriKodu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BaslangicTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BitisTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Durum = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UrunID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IsEmirleri", x => x.IsEmriID);
                    table.ForeignKey(
                        name: "FK_IsEmirleri_Urunler_UrunID",
                        column: x => x.UrunID,
                        principalTable: "Urunler",
                        principalColumn: "UrunID");
                });

            migrationBuilder.InsertData(
                table: "IsEmirleri",
                columns: new[] { "IsEmriID", "Aciklama", "BaslangicTarihi", "BitisTarihi", "Durum", "IsEmriKodu", "UrunID" },
                values: new object[,]
                {
                    { 1, "ABC123 seri nolu ürünün üretimi", new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tamamlandı", "WO-2024-001", 1 },
                    { 2, "XYZ789 seri nolu ürünün üretimi", new DateTime(2024, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Devam Ediyor", "WO-2024-002", 2 },
                    { 3, "DEF456 seri nolu ürünün üretimi", new DateTime(2024, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Beklemede", "WO-2024-003", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_IsEmirleri_UrunID",
                table: "IsEmirleri",
                column: "UrunID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IsEmirleri");
        }
    }
}
