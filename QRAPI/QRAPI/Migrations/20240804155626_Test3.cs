using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QRAPI.Migrations
{
    public partial class Test3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "CategoryId",
                table: "QRs",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "QRs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProductType",
                table: "QRs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CategoryID = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cars_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QRs_CategoryId",
                table: "QRs",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_CategoryID",
                table: "Cars",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_QRs_Categories_CategoryId",
                table: "QRs",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QRs_Categories_CategoryId",
                table: "QRs");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_QRs_CategoryId",
                table: "QRs");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "QRs");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "QRs");

            migrationBuilder.DropColumn(
                name: "ProductType",
                table: "QRs");
        }
    }
}
