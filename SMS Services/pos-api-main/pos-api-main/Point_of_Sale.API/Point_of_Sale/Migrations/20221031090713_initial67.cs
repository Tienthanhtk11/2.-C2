using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point_of_Sale.Migrations
{
    public partial class initial67 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "province_code",
                table: "warehouse",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "province_code",
                table: "partner",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "province_code",
                table: "warehouse");

            migrationBuilder.DropColumn(
                name: "province_code",
                table: "partner");
        }
    }
}
