using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LostDogApp.Migrations
{
    /// <inheritdoc />
    public partial class AddContactNumberToDogReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactNumber",
                table: "LostDogReports",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactNumber",
                table: "LostDogReports");
        }
    }
}
