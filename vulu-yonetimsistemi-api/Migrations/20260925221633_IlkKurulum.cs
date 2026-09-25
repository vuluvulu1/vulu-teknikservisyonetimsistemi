using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vulu_yonetimsistemi_api.Migrations
{
    /// <inheritdoc />
    public partial class IlkKurulum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciAdi = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SifreHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rol = table.Column<int>(type: "int", nullable: false),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Musteriler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdSoyad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Musteriler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teknisyenler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdSoyad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Uzmanlik = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KullaniciId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teknisyenler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teknisyenler_Kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ServisKayitlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MusteriId = table.Column<int>(type: "int", nullable: false),
                    TeknisyenId = table.Column<int>(type: "int", nullable: true),
                    CihazBilgisi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sorun = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Durum = table.Column<int>(type: "int", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TamamlanmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServisKayitlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServisKayitlari_Musteriler_MusteriId",
                        column: x => x.MusteriId,
                        principalTable: "Musteriler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServisKayitlari_Teknisyenler_TeknisyenId",
                        column: x => x.TeknisyenId,
                        principalTable: "Teknisyenler",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Faturalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServisKaydiId = table.Column<int>(type: "int", nullable: false),
                    Tutar = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    OdendiMi = table.Column<bool>(type: "bit", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OdemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faturalar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Faturalar_ServisKayitlari_ServisKaydiId",
                        column: x => x.ServisKaydiId,
                        principalTable: "ServisKayitlari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Randevular",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MusteriId = table.Column<int>(type: "int", nullable: false),
                    TeknisyenId = table.Column<int>(type: "int", nullable: true),
                    ServisKaydiId = table.Column<int>(type: "int", nullable: true),
                    RandevuTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Durum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Randevular", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Randevular_Musteriler_MusteriId",
                        column: x => x.MusteriId,
                        principalTable: "Musteriler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Randevular_ServisKayitlari_ServisKaydiId",
                        column: x => x.ServisKaydiId,
                        principalTable: "ServisKayitlari",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Randevular_Teknisyenler_TeknisyenId",
                        column: x => x.TeknisyenId,
                        principalTable: "Teknisyenler",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Faturalar_ServisKaydiId",
                table: "Faturalar",
                column: "ServisKaydiId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kullanicilar_KullaniciAdi",
                table: "Kullanicilar",
                column: "KullaniciAdi",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Randevular_MusteriId",
                table: "Randevular",
                column: "MusteriId");

            migrationBuilder.CreateIndex(
                name: "IX_Randevular_ServisKaydiId",
                table: "Randevular",
                column: "ServisKaydiId");

            migrationBuilder.CreateIndex(
                name: "IX_Randevular_TeknisyenId",
                table: "Randevular",
                column: "TeknisyenId");

            migrationBuilder.CreateIndex(
                name: "IX_ServisKayitlari_MusteriId",
                table: "ServisKayitlari",
                column: "MusteriId");

            migrationBuilder.CreateIndex(
                name: "IX_ServisKayitlari_TeknisyenId",
                table: "ServisKayitlari",
                column: "TeknisyenId");

            migrationBuilder.CreateIndex(
                name: "IX_Teknisyenler_KullaniciId",
                table: "Teknisyenler",
                column: "KullaniciId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Faturalar");

            migrationBuilder.DropTable(
                name: "Randevular");

            migrationBuilder.DropTable(
                name: "ServisKayitlari");

            migrationBuilder.DropTable(
                name: "Musteriler");

            migrationBuilder.DropTable(
                name: "Teknisyenler");

            migrationBuilder.DropTable(
                name: "Kullanicilar");
        }
    }
}
