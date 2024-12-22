using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace haircare.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ad",
                table: "Islemler",
                newName: "IslemTur");

            migrationBuilder.RenameColumn(
                name: "AdSoyad",
                table: "Calisanlar",
                newName: "Soyad");

            migrationBuilder.AddColumn<string>(
                name: "IslemAd",
                table: "Islemler",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ad",
                table: "Calisanlar",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IslemAd",
                table: "Islemler");

            migrationBuilder.DropColumn(
                name: "Ad",
                table: "Calisanlar");

            migrationBuilder.RenameColumn(
                name: "IslemTur",
                table: "Islemler",
                newName: "Ad");

            migrationBuilder.RenameColumn(
                name: "Soyad",
                table: "Calisanlar",
                newName: "AdSoyad");
        }
    }
}
