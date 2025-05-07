using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LostDogApp.Migrations
{
    /// <inheritdoc />
    public partial class AddImageFieldsToLostDogReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageContentType",
                table: "LostDogReports",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageFileName",
                table: "LostDogReports",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "LostDogReports",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageContentType",
                table: "LostDogReports");

            migrationBuilder.DropColumn(
                name: "ImageFileName",
                table: "LostDogReports");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "LostDogReports");
        }
    }
}
