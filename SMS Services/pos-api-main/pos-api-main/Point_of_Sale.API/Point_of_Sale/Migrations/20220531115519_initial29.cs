using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point_of_Sale.Migrations
{
    public partial class initial29 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_printed",
                table: "product_warehouse",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_weigh",
                table: "product_warehouse",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_printed",
                table: "product_warehouse");

            migrationBuilder.DropColumn(
                name: "is_weigh",
                table: "product_warehouse");
        }
    }
}
