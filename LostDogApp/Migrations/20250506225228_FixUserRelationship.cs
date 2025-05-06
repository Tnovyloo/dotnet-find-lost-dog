using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LostDogApp.Migrations
{
    /// <inheritdoc />
    public partial class FixUserRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LostDogReports_AspNetUsers_UserId1",
                table: "LostDogReports");

            migrationBuilder.DropIndex(
                name: "IX_LostDogReports_UserId1",
                table: "LostDogReports");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "LostDogReports");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "LostDogReports",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_LostDogReports_UserId1",
                table: "LostDogReports",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_LostDogReports_AspNetUsers_UserId1",
                table: "LostDogReports",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
