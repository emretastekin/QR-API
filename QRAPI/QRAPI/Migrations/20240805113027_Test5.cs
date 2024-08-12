using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QRAPI.Migrations
{
    public partial class Test5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocationPlace",
                table: "Tickets",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Place = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Place);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_LocationPlace",
                table: "Tickets",
                column: "LocationPlace");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Locations_LocationPlace",
                table: "Tickets",
                column: "LocationPlace",
                principalTable: "Locations",
                principalColumn: "Place",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Locations_LocationPlace",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_LocationPlace",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "LocationPlace",
                table: "Tickets");
        }
    }
}
