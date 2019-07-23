using Microsoft.EntityFrameworkCore.Migrations;

namespace EbookInfraData.Migrations
{
    public partial class IntialSetup1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ApprovalText",
                table: "ApprovalStatus",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "ApprovalStatus",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "ApprovalStatus");

            migrationBuilder.AlterColumn<string>(
                name: "ApprovalText",
                table: "ApprovalStatus",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 20);
        }
    }
}
