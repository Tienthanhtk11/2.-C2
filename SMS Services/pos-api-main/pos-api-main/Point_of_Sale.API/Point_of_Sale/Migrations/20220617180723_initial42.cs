using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point_of_Sale.Migrations
{
    public partial class initial42 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ecom_id",
                table: "category_unit");

            migrationBuilder.DropColumn(
                name: "ecom_id",
                table: "category_product");

            migrationBuilder.DropColumn(
                name: "ecom_id",
                table: "category_packing");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ecom_id",
                table: "category_unit",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ecom_id",
                table: "category_product",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ecom_id",
                table: "category_packing",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
