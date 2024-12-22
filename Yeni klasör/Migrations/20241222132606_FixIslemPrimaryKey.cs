using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace haircare.Migrations
{
    /// <inheritdoc />
    public partial class FixIslemPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Islemler_IslemId",
                table: "Randevular");

            migrationBuilder.RenameColumn(
                name: "IslemId",
                table: "Randevular",
                newName: "islemId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Randevular",
                newName: "randevuId");

            migrationBuilder.RenameIndex(
                name: "IX_Randevular_IslemId",
                table: "Randevular",
                newName: "IX_Randevular_islemId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Islemler",
                newName: "IslemsId");

            migrationBuilder.RenameColumn(
                name: "Soyad",
                table: "Calisanlar",
                newName: "CalısanSoyad");

            migrationBuilder.RenameColumn(
                name: "Ad",
                table: "Calisanlar",
                newName: "CalısanAd");

            migrationBuilder.AddColumn<string>(
                name: "Durum",
                table: "Randevular",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "musteriId",
                table: "Randevular",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Musteriler",
                columns: table => new
                {
                    musteriId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    kullanıcıAd = table.Column<string>(type: "text", nullable: false),
                    telefonNo = table.Column<string>(type: "text", nullable: false),
                    mail = table.Column<string>(type: "text", nullable: false),
                    sifre = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Musteriler", x => x.musteriId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Randevular_musteriId",
                table: "Randevular",
                column: "musteriId");

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Islemler_islemId",
                table: "Randevular",
                column: "islemId",
                principalTable: "Islemler",
                principalColumn: "IslemsId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Musteriler_musteriId",
                table: "Randevular",
                column: "musteriId",
                principalTable: "Musteriler",
                principalColumn: "musteriId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Islemler_islemId",
                table: "Randevular");

            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Musteriler_musteriId",
                table: "Randevular");

            migrationBuilder.DropTable(
                name: "Musteriler");

            migrationBuilder.DropIndex(
                name: "IX_Randevular_musteriId",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "Durum",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "musteriId",
                table: "Randevular");

            migrationBuilder.RenameColumn(
                name: "islemId",
                table: "Randevular",
                newName: "IslemId");

            migrationBuilder.RenameColumn(
                name: "randevuId",
                table: "Randevular",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Randevular_islemId",
                table: "Randevular",
                newName: "IX_Randevular_IslemId");

            migrationBuilder.RenameColumn(
                name: "IslemsId",
                table: "Islemler",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "CalısanSoyad",
                table: "Calisanlar",
                newName: "Soyad");

            migrationBuilder.RenameColumn(
                name: "CalısanAd",
                table: "Calisanlar",
                newName: "Ad");

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Islemler_IslemId",
                table: "Randevular",
                column: "IslemId",
                principalTable: "Islemler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
