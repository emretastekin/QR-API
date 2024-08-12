using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QRAPI.Migrations
{
    public partial class Test2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "CategoryID",
                table: "Foods",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.CreateIndex(
                name: "IX_Foods_CategoryID",
                table: "Foods",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Foods_Categories_CategoryID",
                table: "Foods",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Foods_Categories_CategoryID",
                table: "Foods");

            migrationBuilder.DropIndex(
                name: "IX_Foods_CategoryID",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "Foods");
        }
    }
}
