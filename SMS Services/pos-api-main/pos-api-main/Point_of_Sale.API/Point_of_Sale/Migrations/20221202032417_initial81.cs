using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point_of_Sale.Migrations
{
    public partial class initial81 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "search_name",
                table: "category_unit",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "search_name",
                table: "category_stalls",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "search_name",
                table: "category_product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "search_name",
                table: "category_packing",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "search_name",
                table: "category_group",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "search_name",
                table: "category_unit");

            migrationBuilder.DropColumn(
                name: "search_name",
                table: "category_stalls");

            migrationBuilder.DropColumn(
                name: "search_name",
                table: "category_product");

            migrationBuilder.DropColumn(
                name: "search_name",
                table: "category_packing");

            migrationBuilder.DropColumn(
                name: "search_name",
                table: "category_group");
        }
    }
}
