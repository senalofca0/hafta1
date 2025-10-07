using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FabrikaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddUrunTipiTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UrunTipiID",
                table: "Urunler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UrunTipleri",
                columns: table => new
                {
                    UrunTipiID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UrunAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UrunKodu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MinKaliteSkoru = table.Column<double>(type: "float", nullable: false),
                    MaxKaliteSkoru = table.Column<double>(type: "float", nullable: false),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunTipleri", x => x.UrunTipiID);
                });

            migrationBuilder.InsertData(
                table: "UrunTipleri",
                columns: new[] { "UrunTipiID", "Aciklama", "AktifMi", "MaxKaliteSkoru", "MinKaliteSkoru", "UrunAdi", "UrunKodu" },
                values: new object[,]
                {
                    { 1, "Standart A tipi ürün", true, 100.0, 80.0, "Standart Ürün A", "STD-A" },
                    { 2, "Premium B tipi ürün", true, 100.0, 90.0, "Premium Ürün B", "PRE-B" },
                    { 3, "Ekonomik C tipi ürün", true, 90.0, 70.0, "Ekonomik Ürün C", "ECO-C" }
                });

            migrationBuilder.UpdateData(
                table: "Urunler",
                keyColumn: "UrunID",
                keyValue: 1,
                column: "UrunTipiID",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Urunler",
                keyColumn: "UrunID",
                keyValue: 2,
                column: "UrunTipiID",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Urunler",
                keyColumn: "UrunID",
                keyValue: 3,
                column: "UrunTipiID",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Urunler",
                keyColumn: "UrunID",
                keyValue: 4,
                column: "UrunTipiID",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Urunler",
                keyColumn: "UrunID",
                keyValue: 5,
                column: "UrunTipiID",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Urunler_UrunTipiID",
                table: "Urunler",
                column: "UrunTipiID");

            migrationBuilder.AddForeignKey(
                name: "FK_Urunler_UrunTipleri_UrunTipiID",
                table: "Urunler",
                column: "UrunTipiID",
                principalTable: "UrunTipleri",
                principalColumn: "UrunTipiID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Urunler_UrunTipleri_UrunTipiID",
                table: "Urunler");

            migrationBuilder.DropTable(
                name: "UrunTipleri");

            migrationBuilder.DropIndex(
                name: "IX_Urunler_UrunTipiID",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "UrunTipiID",
                table: "Urunler");
        }
    }
}
