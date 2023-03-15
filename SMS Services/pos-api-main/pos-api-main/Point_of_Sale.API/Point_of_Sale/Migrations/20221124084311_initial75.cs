using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point_of_Sale.Migrations
{
    public partial class initial75 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "category_group_id",
                table: "product_combo",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "category_id",
                table: "product_combo",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "category_stalls_id",
                table: "product_combo",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "item_code",
                table: "product_combo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "category_group_id",
                table: "product",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "category_id",
                table: "product",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "category_stalls_id",
                table: "product",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "item_code",
                table: "product",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "category_group_id",
                table: "combo",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "category_id",
                table: "combo",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "category_stalls_id",
                table: "combo",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "category_group_id",
                table: "product_combo");

            migrationBuilder.DropColumn(
                name: "category_id",
                table: "product_combo");

            migrationBuilder.DropColumn(
                name: "category_stalls_id",
                table: "product_combo");

            migrationBuilder.DropColumn(
                name: "item_code",
                table: "product_combo");

            migrationBuilder.DropColumn(
                name: "category_group_id",
                table: "product");

            migrationBuilder.DropColumn(
                name: "category_id",
                table: "product");

            migrationBuilder.DropColumn(
                name: "category_stalls_id",
                table: "product");

            migrationBuilder.DropColumn(
                name: "item_code",
                table: "product");

            migrationBuilder.DropColumn(
                name: "category_group_id",
                table: "combo");

            migrationBuilder.DropColumn(
                name: "category_id",
                table: "combo");

            migrationBuilder.DropColumn(
                name: "category_stalls_id",
                table: "combo");
        }
    }
}
