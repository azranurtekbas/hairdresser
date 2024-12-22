using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace haircare.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IslemSuresi",
                table: "Islemler",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UygunlukSaati",
                table: "Calisanlar",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IslemSuresi",
                table: "Islemler");

            migrationBuilder.DropColumn(
                name: "UygunlukSaati",
                table: "Calisanlar");
        }
    }
}
