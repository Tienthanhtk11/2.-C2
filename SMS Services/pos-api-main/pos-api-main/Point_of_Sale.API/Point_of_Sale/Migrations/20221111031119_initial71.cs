using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point_of_Sale.Migrations
{
    public partial class initial71 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "parent_quantity",
                table: "product_structure");

            migrationBuilder.AddColumn<string>(
                name: "packing_code",
                table: "product_structure",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "unit_code",
                table: "product_structure",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "packing_code",
                table: "product_structure");

            migrationBuilder.DropColumn(
                name: "unit_code",
                table: "product_structure");

            migrationBuilder.AddColumn<double>(
                name: "parent_quantity",
                table: "product_structure",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
