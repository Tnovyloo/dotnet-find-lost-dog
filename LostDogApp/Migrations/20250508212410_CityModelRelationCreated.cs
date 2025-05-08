using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LostDogApp.Migrations
{
    /// <inheritdoc />
    public partial class CityModelRelationCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LostDogReports_Cities_CityId",
                table: "LostDogReports");

            migrationBuilder.AddForeignKey(
                name: "FK_LostDogReports_Cities_CityId",
                table: "LostDogReports",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LostDogReports_Cities_CityId",
                table: "LostDogReports");

            migrationBuilder.AddForeignKey(
                name: "FK_LostDogReports_Cities_CityId",
                table: "LostDogReports",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");
        }
    }
}
