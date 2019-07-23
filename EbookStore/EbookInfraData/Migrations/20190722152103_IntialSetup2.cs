using Microsoft.EntityFrameworkCore.Migrations;

namespace EbookInfraData.Migrations
{
    public partial class IntialSetup2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "ApprovalStatus");

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "Book",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "Book");

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "ApprovalStatus",
                maxLength: 500,
                nullable: true);
        }
    }
}
