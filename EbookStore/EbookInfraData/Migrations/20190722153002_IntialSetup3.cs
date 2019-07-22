using Microsoft.EntityFrameworkCore.Migrations;

namespace EbookInfraData.Migrations
{
    public partial class IntialSetup3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_ApprovalStatus_ApprovalId",
                table: "Book");

            migrationBuilder.RenameColumn(
                name: "ApprovalId",
                table: "Book",
                newName: "StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Book_ApprovalId",
                table: "Book",
                newName: "IX_Book_StatusId");

            migrationBuilder.RenameColumn(
                name: "ApprovalText",
                table: "ApprovalStatus",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "ApprovalId",
                table: "ApprovalStatus",
                newName: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_ApprovalStatus_StatusId",
                table: "Book",
                column: "StatusId",
                principalTable: "ApprovalStatus",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_ApprovalStatus_StatusId",
                table: "Book");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "Book",
                newName: "ApprovalId");

            migrationBuilder.RenameIndex(
                name: "IX_Book_StatusId",
                table: "Book",
                newName: "IX_Book_ApprovalId");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "ApprovalStatus",
                newName: "ApprovalText");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "ApprovalStatus",
                newName: "ApprovalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_ApprovalStatus_ApprovalId",
                table: "Book",
                column: "ApprovalId",
                principalTable: "ApprovalStatus",
                principalColumn: "ApprovalId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
